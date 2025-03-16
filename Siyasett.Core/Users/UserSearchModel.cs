using Siyasett.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Users
{
    public class UserSearchModel : SearchModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CreateDate { get; set; }
        public int UserTypeId { get; set; }
        public int Role { get; set; }
    }
}
