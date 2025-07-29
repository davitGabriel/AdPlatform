using AdPlatform.Application.Interfaces;
using AdPlatform.Domain.Entities;

namespace AdPlatform.Infrastructure
{
    public class AdPlatformRepository : IAdPlatformRepository
    {
        private Dictionary<Platform, List<Location>> _platformsDictionary;

        public IEnumerable<Platform> GetAdPlatformsByLocation(string location)
        {
            var parts = location.ToLower().Split('/', StringSplitOptions.RemoveEmptyEntries);
            var prefixes = new HashSet<string>();

            for (int i = parts.Length; i > 0; i--)
            {
                string prefix = "/" + string.Join("/", parts.Take(i));
                prefixes.Add(prefix);
            }

            List<Platform> matchingPlatforms = new();

            foreach (var kvp in _platformsDictionary)
            {
                var platform = kvp.Key;
                var locations = kvp.Value;

                if (locations.Any(loc => prefixes.Contains(loc.Name)))
                {
                    matchingPlatforms.Add(platform);
                }
            }

            return matchingPlatforms;
        }

        public Dictionary<Platform, List<Location>> AddLoadedDataToDb(Dictionary<string, string[]> platformsDetails)
        {
            _platformsDictionary = new Dictionary<Platform, List<Location>>();

            foreach (var platformEntry in platformsDetails)
            {
                var platformName = platformEntry.Key;
                var locationPaths = platformEntry.Value;

                var platform = new Platform
                {
                    Name = platformName,
                    PlatformLocations = new List<Location>()
                };

                foreach (var path in locationPaths.Distinct())
                {
                    if (!platform.PlatformLocations.Any(pl => pl.Name == path))
                    {
                        var platformLocation = new Location
                        {
                            Name = path.ToLower(),
                            Platform = platform
                        };

                        platform.PlatformLocations.Add(platformLocation);
                    }
                }

                _platformsDictionary.Add(platform, platform.PlatformLocations);
            }

            return _platformsDictionary;
        }
    }
}
