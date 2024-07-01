using Comic.Application.IServices;
using Comic.Application.Services;
using Comic.Domain.Common;
using Comic.Domain.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Comic.WebAPI.Filters;
using Comic.WebAPI.Common;

namespace Comic.WebAPI.Controllers
{
    [Route("api/seed-data")]
    [ApiController]
    public class SeedDataController : ControllerBase
    {
        private readonly ISeedDataService _seedDataService;
        private readonly ILogger<SeedDataController> _logger;
        private readonly IAuthService _authService;

        public SeedDataController(ISeedDataService seedDataService, ILogger<SeedDataController> logger
            , IAuthService authService)
        {
            _seedDataService = seedDataService;
            _logger = logger;
            _authService = authService;
        }

        
        [HttpGet("init")]
        [Authorization("sdsd","sdsd")]
        public async Task<IActionResult> InitAsync()
        {
            await _seedDataService.InitSeedDataAsync();

            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Seed data successfully", null));
        }
    }
}
