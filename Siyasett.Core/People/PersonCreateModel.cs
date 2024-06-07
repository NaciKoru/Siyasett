using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.People
{
    public class PersonCreateModel : BaseModel
    {

        [Required(ErrorMessage = "Kişi adı zorunludur.")]
        public string? FirstName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Kişi soyadı zorunludur.")]
        public string? LastName { get; set; } = String.Empty;
        public string? DateOfBirth { get; set; }

        public string? PlaceOfBirth { get; set; }
        public int GenderId { get; set; } = 1;
        public int EducationId { get; set; } = 1;
        public int? PartyId { get; set; }
        public int[]? JobIds { get; set; }
        //public int[] PositionIds { get; set; }
        public int InstitutionTypeId { get; set; } = 1;

        public int PositionFieldId { get; set; } = 1;
        public string? CvTr { get; set; }
        public string? Photo { get; set; }
        public string? UploadPhoto { get; set; }
        public string? CvEn { get; set; }
        public bool IsActive { get; set; } = false;

    }

}
