using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Users
{
    public class UserRoleModel
    {

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public bool Selected { get; set; }

    }
}
