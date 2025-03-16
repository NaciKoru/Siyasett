using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.People
{

    public class PersonDetailModel : BaseModel
    {

        [Required(ErrorMessage = "Kişi adı zorunludur.")]
        public string? FirstName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Kişi soyadı zorunludur.")]
        public string? LastName { get; set; } = String.Empty;
        public string? DateOfBirth { get; set; }

        public string? PlaceOfBirth { get; set; }
        public int GenderId { get; set; } = 1;
        public string GenderName { get; set; }
        public string GenderNameEn { get; set; }


        public int EducationId { get; set; } = 1;
        public string EducationName { get; set; }
        public string EducationNameEn { get; set; }


        public int? PartyId { get; set; }
        public string? PartyName { get; set; }
        public string? PartyNameEn { get; set; }

        public int[] JobIds { get; set; }
        public string JobNames { get; set; }
        public string JobNamesEn { get; set; }

        public int PositionFieldId { get; set; } = 1;
        public string PositionFieldName { get; set; }
        public string PositionFieldNameEn { get; set; }


        public string? InstitutionType { get; set; }
        public string? InstitutionName { get; set; }
        public string? InstitutionNameEn { get; set; }


        public string? PositionName { get; set; }
        public string? PositionNameEn { get; set; }

        public string? Sector { get; set; }
        public string? SectorEn { get; set; }


        public string? CvTr { get; set; }

        public string? Photo { get; set; }

        public string? ProvinceName { get; set; }
        public string? DistrictName { get; set; }

        public string? CvEn { get; set; }
        public bool IsActive { get; set; } = false;

    }
}
