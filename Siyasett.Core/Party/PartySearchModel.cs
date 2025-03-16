using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Party
{
    public class PartySearchModel
    {
        public string PartyName { get; set; }
        public string PartyShortName { get; set; }
        public string PartyLeader { get; set; }
        public string PartyDof { get; set; }
        public int PartyParliamentery { get; set; }
        public int PartyMember { get; set; }
        public string WebSite { get; set; }
        public int Active { get; set; }
        public int Enjoyable { get; set; }

        public byte OrderBy { get; set; }
        public byte OrderByDesc { get; set; }
    }
}
