using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Poll
{
    public class PollResultListModel
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public double Ratio { get; set; }
        public int PollId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
