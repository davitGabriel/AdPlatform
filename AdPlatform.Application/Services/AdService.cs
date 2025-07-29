using AdPlatform.Api.Controllers;
using AdPlatform.Application.Interfaces;
using AdPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace AdPlatform.Application.Services
{
    public class AdService : IAdService
    {
        private readonly IAdPlatformRepository _adPlatformRepository;

        public AdService(IAdPlatformRepository adPlatformRepository)
        {
            _adPlatformRepository = adPlatformRepository;
        }

        public IEnumerable<Platform> GetAddPlatformsByLocation(string location)
        {
            var foundPlatforms = _adPlatformRepository.GetAdPlatformsByLocation(location);

            return foundPlatforms ?? Enumerable.Empty<Platform>();
        }

        public Dictionary<Platform, List<Location>> LoadAdPlatformsFromFile(IFormFile file)
        {   
            string readContents;
            using (var streamReader = new MemoryStream())
            {
                file.CopyTo(streamReader);
                var fileBytes = streamReader.ToArray();

                readContents = System.Text.Encoding.UTF8.GetString(fileBytes);
            }

            if (string.IsNullOrEmpty(readContents))
            {
                throw new InvalidOperationException("The file is empty or not found.");
            }

            var pattern = new Regex(@"([\p{L}\s\.]+)\s*:\s*([\w\/,]+)");
            var platformsDetails = new Dictionary<string, string[]>();

            MatchCollection matches = pattern.Matches(readContents);

            foreach (Match match in matches)
            {
                string platform = match.Groups[1].Value.Trim();
                string locationsRaw = match.Groups[2].Value.Trim();

                string[] locations = locationsRaw.Split(',')
                                                 .Select(loc => loc.Trim())
                                                 .ToArray();

                platformsDetails.TryAdd(platform, locations);
            }

            var loadedPlatforms = _adPlatformRepository.AddLoadedDataToDb(platformsDetails);

            return loadedPlatforms;
        }
    }
}
