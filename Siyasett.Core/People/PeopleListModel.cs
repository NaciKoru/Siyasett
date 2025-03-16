using Siyasett.Core.SocialMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.People
{
    public class PeopleListModel:BaseModel
    {
        public string FirstName { get; set; }=String.Empty;
        public string LastName { get; set; }= String.Empty;
        
        public int GenderId { get; set; }
        public string GenderName { get; set; } = String.Empty; 
        public string GenderNameEn { get; set; } = String.Empty;
        public string Photo { get; set; } = String.Empty;
        public int EducationId { get; set; }
        public string EducationName { get; set; } = String.Empty;
        public string EducationNameEn { get; set; } = String.Empty;
        public bool IsActive { get; set; } = false;
        public string PartyName { get; set; } = String.Empty;
        public string PartyNameEn { get; set; } = String.Empty;
        public string Positions { get; set; } = String.Empty;
        public string ProvinceName { get; set; } = String.Empty;    
        public string DistrictName { get; set; } = String.Empty;
        public string PositionName { get; set; } = String.Empty;
        public string PositionNameEn { get; set; } = String.Empty;
        public string InstitutionName { get; set; } = String.Empty;
        public string InstitutionNameEn { get; set; } = String.Empty;
        public string PlaceOfBirth { get; set; }
        public string DateOfBirth { get; set; }
        public string Period { get; set; }
        public string InstitutionTypeName { get; set; } = string.Empty;
        public string InstitutionTypeNameEn { get; set; } = string.Empty;
        public int Views { get; set; }
        public int? PositionWeight { get; set; }
        public int Weight { get; set; }
        public int? PositionSector { get; set; }
        public string PositionSectorName { get; set; }
        public string PositionSectorNameEn { get; set; }
        public int? PositionId { get; set; }
        public int? PartyId { get; set; }
        public int? SectorId { get; set; }
        public string? SectorName { get; set; }
        public string? SectorNameEn{ get; set; }
        public int PhotoOrder { get; set; }
        public int JobProvinceId { get; set; }
        public int JobId { get; set; }
        public List<SocialMediaModel> SocialMedias{ get; set; }
        public List<PeoplePositionModel> PeoplePositions { get; set; }
    }
}
