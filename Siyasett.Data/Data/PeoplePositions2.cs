using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data
{
    public partial class PeoplePositions2
    {
        public int Id { get; set; }
        public int PeopleId { get; set; }
        public int InstitutionTypeId { get; set; }
        public string? InstitutionName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? PartyId { get; set; }
        public int PositionId { get; set; }
        public int? Period { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
