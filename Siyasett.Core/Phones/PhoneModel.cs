using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Phones
{
    public class PhoneModel
    {
        public int PhoneId { get; set; }
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }
        public int PhoneTypeId { get; set; }
        public string? PhoneTypeNameTr { get; set; }
        public string? PhoneTypeNameEn { get; set; }
        public int CommunicationTypeId { get; set; }
        public string? CommunicationTypeNameTr { get; set; }
        public string? CommunicationTypeNameEn { get; set; }

        public string? CountryCode { get; set; }
        public int CountryId { get; set; }

        public int PeopleId { get; set; }

        public string PrefixCode { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }

    }
}
