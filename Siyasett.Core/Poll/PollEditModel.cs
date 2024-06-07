using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Poll
{
    public class PollEditModel
    {
        public int Id { get; set; }
        public int PollCompanyId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int PollTypeId { get; set; }
        public int PollKindId { get; set; }
        public int SampleSize { get; set; } = 0;
        public bool Active { get; set; } = true;
        public List<PollResultEditModel> Results { get; set; }
        public int? Day { get; set; }
        public PollEditModel()
        {
            Results = new();
        }
    }
}
