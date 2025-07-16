using FluentValidation;

namespace MinimalApi;

public class CouponCreateValidation : AbstractValidator<CreateCouponRequest>
{
  public CouponCreateValidation()
  {
    RuleFor(model => model.Name).NotEmpty()
    .MaximumLength(100);
    RuleFor(model => model.Percent)
    .InclusiveBetween(1, 100);
  }
}
