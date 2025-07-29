using AdPlatform.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPlatform.Domain.Entities
{
    public class Location : BaseEntity
    {
        public Platform Platform { get; set; }
        public string Name { get; set; }
    }
}
