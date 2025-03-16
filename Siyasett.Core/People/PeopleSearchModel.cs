using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.People
{
    public class PeopleSearchModel
    {
        
        public byte GenderId { get; set; }
        public int EducationId { get; set; }
        public int InstitutionTypeId { get; set; }
        public int PartyId { get; set; }
        public int PositionId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Period { get; set; }
        public int IsActivePolitics { get; set; }
        public byte OrderBy { get; set; }
        public byte OrderByDesc { get; set; }

        public int Sectors { get; set; }
        public int Province { get; set; }
        public int District { get; set; }
        public int Job { get; set; }
        public string? Birtplace { get; set; }
        public int Agegap { get; set; }
        public int IsAlive { get; set; }
        public string ProvinceName { get; set; }
    }
}
