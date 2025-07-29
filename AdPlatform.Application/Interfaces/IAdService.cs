using AdPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AdPlatform.Api.Controllers
{
    public interface IAdService
    {
        public Dictionary<Platform, List<Location>> LoadAdPlatformsFromFile(IFormFile file);

        public IEnumerable<Platform> GetAddPlatformsByLocation(string location);
    }
}