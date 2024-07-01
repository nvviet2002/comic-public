using Comic.Application.IServices;
using Comic.Application.Services;
using Comic.WebAPI.Common;
using Comic.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Comic.WebAPI.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {


        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file, string fileName)
        {
            var awsS3Service = new AwsS3Service();
            await awsS3Service.UploadImageAsync(file, fileName, "aaaa");

            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "test successfully", null));
        }

        [HttpPost("create-folder")]
        public async Task<IActionResult> CreateFolderAsync(string folderName)
        {
            var awsS3Service = new AwsS3Service();
            await awsS3Service.CreateFolderAsync(folderName,"uploads/story/");

            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "test successfully", null));
        }

        [HttpPost("delete-folder")]
        public async Task<IActionResult> DeleteFolderAsync(string folderName)
        {
            var awsS3Service = new AwsS3Service();
            await awsS3Service.DeleteFolderAsync(folderName, "uploads/story/");

            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "test successfully", null));
        }
    }
}
