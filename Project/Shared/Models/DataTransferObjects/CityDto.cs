using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DataTransferObjects
{
    public class CityDto
    {
        [Length(2, 50), RegularExpression("^(?:[A-Za-z]+)$")]
        public string Name { get; set; }
    }
}
