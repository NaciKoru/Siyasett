
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Data
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdate { get; set; }

        public int UserTypeId { get; set; } = 1;
        public Nullable<DateTime> EndDateOfMembership { get; set; } = null;

        
        public string? Token { get; set; }
        public AppUser() : base()
        {
            CreateDate = DateTime.Now;
            LastUpdate = DateTime.Now;
            this.TwoFactorEnabled = true;
            
        }

    }
}
