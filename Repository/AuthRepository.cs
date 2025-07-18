
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MinimalApi;

public class AuthRepository : IAuthRepository
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly ApplicationDbContext _context;
  private readonly IMapper _mapper;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly IConfiguration _configuration;
  private readonly string secretKey;
  private readonly ILogger<AuthRepository> logger;
  public AuthRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthRepository> logger)
  {
    _userManager = userManager;
    _context = context;
    _mapper = mapper;
    _roleManager = roleManager;
    _configuration = configuration;
    secretKey = _configuration.GetValue<string>("ApiSettings:Secret") ?? throw new Exception("Secret key is null");
    this.logger = logger;
  }
  public async Task<bool> IsUniqueUser(string email)
  {
    return !await _userManager.Users.AnyAsync(x => x.Email == email);
  }

  public async Task<LoginUserResponse> LoginUser(LoginUserRequest request)
  {
    // ? check is email is exist
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user is null)
      throw new AuthException("Invalid User Credentials", StatusCodes.Status400BadRequest);
    // ? check if match password
    if (!await _userManager.CheckPasswordAsync(user, request.Password))
      throw new AuthException("Invalid User Credentials", StatusCodes.Status400BadRequest);

    try
    {
      var roles = await _userManager.GetRolesAsync(user);

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(secretKey);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]{
        new Claim(ClaimTypes.Name, user.Email!),
        new Claim(ClaimTypes.Role, roles.FirstOrDefault()!)
      })
      ,
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);

      return new LoginUserResponse()
      {
        User = _mapper.Map<UserDto>(user),
        Token = new JwtSecurityTokenHandler().WriteToken(token)
      };
    }
    catch (Exception ex)
    {
      logger.LogError(ex.Message);
      throw new AuthException("Error While Login Process", StatusCodes.Status500InternalServerError);
    }

  }

  public async Task RegisterUser(RegisterUserRequest request)
  {
    if (!await IsUniqueUser(request.Email))
      throw new UserAlreadyExistsException();

    var appUser = _mapper.Map<ApplicationUser>(request);

    var result = await _userManager.CreateAsync(appUser, request.Password);
    if (!result.Succeeded)
    {
      var errors = string.Join(", ", result.Errors.Select(x => x.Description));
      throw new AuthException(errors);
    }
    // ? create an admin role
    if (!await _roleManager.RoleExistsAsync("Admin"))
    {
      await _roleManager.CreateAsync(new IdentityRole("Admin"));
      await _roleManager.CreateAsync(new IdentityRole("User"));
    }

    // ? Assign role for first user
    var role = _userManager.Users.Any() == false ? "Admin" : "User";

    var ruleResult = await _userManager.AddToRoleAsync(appUser, role);

    if (!ruleResult.Succeeded)
    {
      var errors = string.Join(", ", ruleResult.Errors.Select(x => x.Description));
      throw new AuthException(errors);
    }
  }

  public async Task<AssignAdminResponse> AssignAdminRole(ApplicationUser currentUser, AssignAdminRequest request)
  {

    if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
      throw new AuthException("User is unauthorized to perform this action", StatusCodes.Status401Unauthorized);

    var user = await _userManager.FindByIdAsync(request.userId);
    if (user is null)
      throw new AuthException("User Not Found!", StatusCodes.Status404NotFound);

    if (!await _roleManager.RoleExistsAsync(request.Role))
      throw new AuthException("Role doesn't exists", StatusCodes.Status400BadRequest);

    var result = await _userManager.AddToRoleAsync(user, request.Role);

    if (!result.Succeeded)
    {
      var errors = string.Join(", ", result.Errors.Select(x => x.Description));
      throw new AuthException(errors, StatusCodes.Status500InternalServerError);
    }

    return new AssignAdminResponse()
    {
      Code = StatusCodes.Status200OK,
      Message = "Assign Role Donee.."
    };
  }

  public async Task<ICollection<ApplicationUser>> getUsers()
  {
    return await _userManager.Users.ToListAsync();
  }
}
