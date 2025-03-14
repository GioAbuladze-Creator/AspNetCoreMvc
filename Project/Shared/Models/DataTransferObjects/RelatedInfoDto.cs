using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DataTransferObjects
{
    public class RelatedInfoDto
    {
        public string Name { get; set; }
        public Dictionary<string, string> RelatedInfo { get; set; }

    }
}
