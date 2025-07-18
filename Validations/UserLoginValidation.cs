using FluentValidation;

namespace MinimalApi;

public class UserLoginValidation: AbstractValidator<LoginUserRequest>
{
  public UserLoginValidation()
  {
    RuleFor(x => x.Email)
    .NotEmpty().EmailAddress();

    RuleFor(x => x.Password)
    .NotEmpty();
  }
}
