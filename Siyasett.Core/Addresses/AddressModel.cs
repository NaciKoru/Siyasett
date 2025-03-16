using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Addresses
{
    public class AddressModel:BaseModel
    {
        public string NameEn { get; set; }
        public string NameTr { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressTypeNameTr { get; set; }
        public string AddressTypeNameEn { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string District{ get; set; }
        public string Province { get; set; }
        public int CountryId { get; set; }

        public string? CountryNameTr{ get; set; }
        public string? CountryNameEn{ get; set; }
        public string PostalCode { get; set; }
        public int peopleId { get; set; }
        public int partyId { get; set; }

        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
    }
}
