using AdPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdService _adService;
        public AdController(IAdService adService)
        {
            _adService = adService;
        }

        [HttpPost("LoadAdPlatforms")]
        public IActionResult LoadAdPlatformsFromFile(IFormFile file)
        {
            var platformsLoaded = _adService.LoadAdPlatformsFromFile(file);

            if (platformsLoaded.Count > 0)
            {
                var response = platformsLoaded.ToDictionary(
                            kvp => kvp.Key.Name,
                            kvp => kvp.Value.Select(loc => loc.Name).ToList()
                    );
                return Ok(response);
            }

            return NotFound("No ad platforms found in file.");
        }

        [HttpGet("GetAdPlatformsByLocation")]
        public IActionResult GetAdPlatformsByLocation([FromQuery] string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Ok("Please provide name of location.");

            var platformsFound = _adService.GetAddPlatformsByLocation(input);

            if (platformsFound.Count() > 0)
            {
                var platformsNames = platformsFound.Select(x => x.Name).ToList();
                return Ok(platformsNames);
            }

            return NotFound("No ad platforms found for the specified location.");
        }
    }
}
