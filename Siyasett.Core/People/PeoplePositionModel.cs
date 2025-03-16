using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Siyasett.Core.People
{
    public class PeoplePositionModel
    {
        public int Id { get; set; }
        public int PeopleId { get; set; }
        public int InstitutionTypeId { get; set; }
        public string InstitutionTypeName { get; set; }
        public string InstitutionTypeNameEn { get; set; }

        public string? InstitutionName { get; set; }
        public string? InstitutionNameEn { get; set; }



        [IgnoreDataMember]
        public DateOnly? StartDate
        {
            get
            {
                if (!StartYear.HasValue)
                    return null;
                try
                {
                    int startDay = this.StartDay ?? 1;
                    int startMonth = this.StartMonth ?? 1;
                    DateOnly dateOnly = new DateOnly(this.StartYear.Value, startMonth, startDay);
                    return dateOnly;
                }
                catch
                {
                    return null;
                }
            }
        }

        [IgnoreDataMember]

        public DateOnly? EndDate
        {
            get
            {
                if (!EndYear.HasValue)
                    return null;
                try
                {
                    int startDay = this.EndDay ?? 1;
                    int startMonth = this.EndMonth ?? 1;
                    DateOnly dateOnly = new DateOnly(this.EndYear.Value, startMonth, startDay);
                    return dateOnly;
                }
                catch
                {
                    return null;
                }
            }
        }


        
        public int? PartyId { get; set; }
        public string? PartyName { get; set; }
        public string? PartyNameEn { get; set; }

        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionNameEn { get; set; }

        public string? Period { get; set; }
        public string? Description { get; set; }
        public string? DescriptionEn { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public short? StartDay { get; set; }
        public short? StartMonth { get; set; }
        public short? StartYear { get; set; }
        public short? EndDay { get; set; }
        public short? EndMonth { get; set; }
        public short? EndYear { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? SectorId { get; set; }
        public string? SectorName { get; set; }
        public string? SectorNameEn { get; set; }

        public string? ProvinceName { get; set; }
        public string? DistrictName { get; set; }

    }
}
