using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Party
{
    public class PartyTextListModel
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public int SectorId { get; set; }
        public string Text { get; set; }
        public string SectorName { get; set; }
    }
}
