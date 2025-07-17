using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi;

public static class CouponEndpoint
{

  public static void MapCouponEndpoints(this IEndpointRouteBuilder app)
  {
    // Define your coupon-related endpoints here

    var group = app.MapGroup("/api/v1/coupons")
      .WithTags("Coupons")
      .WithSummary("Coupon Management API")
      .WithDescription("Endpoints for managing coupons in the application");


    group.MapGet("/", GetAllCoupons)
    .WithName("GetCoupons")
    .Produces<BaseResponse<CouponDto>>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .WithOpenApi(operation =>
    {
      operation.Summary = "Get all coupons";
      operation.Description = "Retrieves a list of all available coupons.";
      return operation;
    });

    group.MapGet("/{id:int}", GetCouponById)
    .WithName("GetCoupon")
    .AddEndpointFilter<ParameterIdValidator>()
    .Produces<BaseResponse<CouponDto>>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .Produces(StatusCodes.Status400BadRequest);


    group.MapPost("/", CreateCoupon)
    .WithName("CreateCoupon")
    .AddEndpointFilter<BasicValidator<CreateCouponRequest>>();

    group.MapPatch("/{id:int}", UpdateCoupon)
    .WithName("UpdateCoupon")
    .AddEndpointFilter<BasicValidator<UpdateCouponRequest>>();

    group.MapDelete("/{id:int}", DeleteCoupon)
    .WithName("DeleteCoupon");
  
  }

  private static async Task<IResult> DeleteCoupon([FromRoute] int id, ICouponRepository couponRepo, ILogger<Program> logger)
  {
    logger.LogInformation("Delete Coupon Handler");
    try
    {
      var coupon = await couponRepo.GetAsync(id);
      if (coupon is null)
        return TypedResults.NotFound("Invalid Id");

      await couponRepo.DeleteAsync(coupon);
      return TypedResults.Ok("Coupon Delete Successfully");
    }
    catch (Exception ex)
    {
      logger.LogError(ex.Message);
      return TypedResults.Problem(detail: "Error while delete Coupon", statusCode: StatusCodes.Status500InternalServerError);
    }
  }
  private static async Task<IResult> UpdateCoupon([FromRoute] int id, UpdateCouponRequest request, ICouponRepository couponRepo, IMapper mapper, ILogger<Program> logger)
  {
    logger.LogInformation("Update Coupon Handler");
    try
    {
      if (couponRepo.GetAsync(id).GetAwaiter().GetResult() is null)
      {
        return TypedResults.NotFound("Invalid id");
      }
      var coupon = mapper.Map<Coupon>(request);
      await couponRepo.UpdateAsync(coupon);

      return TypedResults.Ok("Coupon Update Successfully");
    }
    catch (Exception ex)
    {
      logger.LogError(ex.Message);
      return TypedResults.Problem(detail: "Error while update coupon", statusCode: StatusCodes.Status500InternalServerError);
    }
  }
  private static async Task<IResult> CreateCoupon([FromBody] CreateCouponRequest request, ICouponRepository couponRepo, IMapper mapper, ILogger<Program> logger)
  {
    logger.LogInformation("Create Coupon Handlers.......");
    try
    {
      // * check if coupon is exist or not
      var isCouponExist = await couponRepo.GetByCodeAsync(request.Name);

      if (isCouponExist is not null)
      {
        return TypedResults.BadRequest(
          new BaseResponse<CouponDto>()
          {
            Success = false,
            Error = "Coupon is already exists"
          });
      }
      var coupon = mapper.Map<Coupon>(request);
      await couponRepo.CreateAsync(coupon);
      await couponRepo.SaveAsync();



      return TypedResults.Created(
        "Created Coupon Successfully",
        new BaseResponse<CouponDto>()
        {
          Success = true,
          Data = mapper.Map<CouponDto>(coupon)
        });
    }
    catch (Exception ex)
    {
      logger.LogError(ex.Message);
      return Results.Problem("Error while Create Coupon", statusCode: StatusCodes.Status500InternalServerError);
    }
  }
  private static async Task<IResult> GetCouponById(int id, ICouponRepository couponRepo, IMapper mapper,ILogger<Program> logger)
  {
    logger.LogInformation("get coupon using id executed....");
    try
    {
      var coupon = await couponRepo.GetAsync(id);
      if (coupon is null)
      {
        return Results.BadRequest(new BaseResponse<CouponDto>()
        {
          Success = false,
          Error = "Invalid id",
          Message = "No Coupon with this Id"
        });
      }
      return Results.Ok(new BaseResponse<CouponDto>()
      {
        Success = true,
        Message = "Coupon Retrieve Successfully",
        Data = mapper.Map<CouponDto>(coupon)
      });
    }
    catch (Exception e)
    {
      logger.LogError(e, "Error while get Coupon");
      return Results.Problem(
        detail: "Error while get Coupon, Please try again letter", statusCode: StatusCodes.Status500InternalServerError
        );
    }
  }
  private static async Task<IResult> GetAllCoupons(ICouponRepository couponRepo, IMapper mapper,ILogger<Program> logger)
  {
    logger.LogInformation("Fetching all coupons from the repository...");

    try
    {
      var coupons = await couponRepo.GetAllCouponsAsync();
      var response = new BaseResponse<CouponDto>
      {
        ArrayData = mapper.Map<List<CouponDto>>(coupons),
        Message = "Coupons retrieved successfully",
        Success = true
      };

      return TypedResults.Ok(response);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred while fetching coupons.");
      return TypedResults.Problem(
        title: "An error occurred while retrieving coupons.",
      statusCode: StatusCodes.Status500InternalServerError
      );
    }

  }

}
