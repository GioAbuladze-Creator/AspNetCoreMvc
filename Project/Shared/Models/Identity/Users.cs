using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Shared.Models.Identity
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
