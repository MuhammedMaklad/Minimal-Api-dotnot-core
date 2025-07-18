using FluentValidation;

namespace MinimalApi;

public class UserRegisterValidation : AbstractValidator<RegisterUserRequest>
{
  public UserRegisterValidation()
  {
    RuleFor(x => x.UserName)
    .NotEmpty().MinimumLength(3).MaximumLength(20);

    RuleFor(x => x.Email)
    .NotEmpty().EmailAddress();

    RuleFor(x => x.Password).
    NotEmpty().MinimumLength(6);
    
  }
}
