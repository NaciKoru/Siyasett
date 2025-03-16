using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Party
{
    public class PartyIdeolojiModel:BaseModel
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public int IdeologyId { get; set; }
        public int Value { get; set; }
        public string PartyShortName { get; set; }
        public string PartyName { get; set; }
        public string IdeologyName { get; set; }

    }
}
