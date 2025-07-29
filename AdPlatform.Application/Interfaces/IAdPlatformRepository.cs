using AdPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPlatform.Application.Interfaces
{
    public interface IAdPlatformRepository
    {
        Dictionary<Platform, List<Location>> AddLoadedDataToDb(Dictionary<string, string[]> platforms);
        IEnumerable<Platform> GetAdPlatformsByLocation(string location);
    }
}
