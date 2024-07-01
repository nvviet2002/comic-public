using Comic.Domain.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace Comic.WebAPI.Common
{
    public class ModelStateHelper
    {
        public static IActionResult ModelStateResponse(ActionContext actionContext)
        {
            var errorDic = new Dictionary<string, List<string>>();
            foreach(var errors in actionContext.ModelState)
            {
                var newErrors = new List<string>();
                foreach(var error in errors.Value.Errors)
                {
                    newErrors.Add(error.ErrorMessage);
                }

                errorDic.Add(errors.Key, newErrors);
            }

            return new BadRequestObjectResult(new ApiResponse<dynamic>()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Tham số không đúng định dạng",
                Data = errorDic,
            });
        }
    }
}
