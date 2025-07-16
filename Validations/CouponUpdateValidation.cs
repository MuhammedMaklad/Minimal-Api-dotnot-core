using FluentValidation;

namespace MinimalApi;

public class CouponUpdateValidation : AbstractValidator<UpdateCouponRequest>
{
  public CouponUpdateValidation()
  {
    RuleFor(model => model.Name).NotEmpty();
    RuleFor(model => model.Percent).InclusiveBetween(1, 100);
    RuleFor(model => model.IsActive).NotEmpty();
  }
}
