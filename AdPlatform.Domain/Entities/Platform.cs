using AdPlatform.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPlatform.Domain.Entities
{
    public class Platform : BaseEntity
    {
        public string Name { get; set; }
        public List<Location> PlatformLocations{ get; set; }
    }
}
