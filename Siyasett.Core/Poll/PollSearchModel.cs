using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Poll
{
    public class PollSearchModel
    {

        public int CompanyId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int SampleSize { get; set; }
        public double PartyRatio { get; set; }
        public byte OrderBy { get; set; }
        public byte OrderByDesc { get; set; }
    }
}
