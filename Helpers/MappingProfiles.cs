using AutoMapper;

namespace MinimalApi;

public class MappingProfiles : Profile
{
  public MappingProfiles()
  {
    CreateMap<CreateCouponRequest, Coupon>().ReverseMap();
    CreateMap<Coupon, CouponDto>().ReverseMap();
    CreateMap<Coupon, UpdateCouponRequest>().ReverseMap();
    CreateMap<ApplicationUser, RegisterUserRequest>().ReverseMap();
    CreateMap<UserDto, ApplicationUser>().ReverseMap();
  }
}
