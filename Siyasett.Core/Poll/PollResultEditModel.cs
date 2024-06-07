using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Poll
{
    public class PollResultEditModel
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public double Ratio { get; set; }
        public string? PartyName { get; set; }


    }
}
