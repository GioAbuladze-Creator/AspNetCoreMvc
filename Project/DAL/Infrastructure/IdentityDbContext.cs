using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Identity;

namespace DAL.Infrastructure
{
    public class IdentityDbContext : IdentityDbContext<Users>
    {
        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
