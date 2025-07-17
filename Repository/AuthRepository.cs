
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
  public AuthRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
  {
    _userManager = userManager;
    _context = context;
    _mapper = mapper;
    _roleManager = roleManager;
    _configuration = configuration;
    secretKey = configuration.GetValue<string>("ApiSettings:Secret") ?? throw new Exception("Secret key is null");
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
      return null!;
    // ? check if match password
    if (await _userManager.CheckPasswordAsync(user, request.Password))
      return null!;

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

  public async Task<bool> RegisterUser(RegisterUserRequest request)
  {
    var appUser = _mapper.Map<ApplicationUser>(request);

    if (!(await _userManager.CreateAsync(appUser, request.Password)).Succeeded)
      return false;

    // ? create an admin if no admin in our system 
    if (!await _roleManager.RoleExistsAsync("Admin"))
    {
      await _roleManager.CreateAsync(new IdentityRole("Admin"));
      await _roleManager.CreateAsync(new IdentityRole("User"));

      await _userManager.AddToRoleAsync(appUser, "Admin");
    }

    return true;
  }

  public async Task<AssignAdminResponse> AssignAdminRole(ApplicationUser currentUser, AssignAdminRequest request)
  {
    
    if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
      return new AssignAdminResponse()
      {
        Code = StatusCodes.Status403Forbidden,
        Message = "User is UnAuthorization to do this Actions"
      };

    var user = await _userManager.FindByIdAsync(request.userId);
    if (user is null)
      return new AssignAdminResponse()
      {
        Code = StatusCodes.Status404NotFound,
        Message = "User is not Found"
      };

    if (!await _roleManager.RoleExistsAsync(request.Role))
      return new AssignAdminResponse()
      {
        Code = StatusCodes.Status400BadRequest,
        Message = "This Role isn't exists"
      };
    if (!(await _userManager.AddToRoleAsync(user, request.Role)).Succeeded)
      return new AssignAdminResponse()
      {
        Code = StatusCodes.Status500InternalServerError,
        Message = "Error While Attach Role"
      };

    return new AssignAdminResponse()
    {
      Code = StatusCodes.Status200OK,
      Message = "Assign Role Donee.."
    };
  }
}
