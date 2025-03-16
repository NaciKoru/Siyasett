using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Users
{
    public class UserEditModel
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }
        [Required]
        public string Email { get; set; }

        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public bool IsActive { get; set; }

        public List<UserRoleModel> Roles { get; set; }
    }
}
