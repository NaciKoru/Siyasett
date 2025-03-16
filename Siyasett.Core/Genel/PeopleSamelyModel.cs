using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Genel
{
    public class PeopleSamelyModel:BaseModel
    {
        public string? PersonFirstName { get; set; }
        public string? PersonLastName { get; set; }
        public string? PersonPosition { get; set; }
        public string? PersonJob { get; set; }
        public string? PersonAreaOfInterest { get; set; }
        public string? PersonParty { get; set; }
        public string? PersonId { get; set; }
        public string? Photo { get; set; }
    }
}
