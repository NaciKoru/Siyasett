using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Poll
{
    public class PollModel:BaseModel
    {
       
        public int PollCompanyId { get; set; }
        public string PollCompanyNameTr { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int PollTypeId { get; set; }
        public string? PollTypeName { get; set; }
        public int PollKindId { get; set; }
        public string? PollKindName { get; set; }
        public int SampleSize { get; set; } = 0;
        public bool Active { get; set; }
        public List<PollResultModel> Results { get; set; }
        public double? Ratio { get; set; }

    }

    public class PollResultModel
    {
        public string PartyName { get; set; }
        public double PartyRatio { get; set; }
        public int PollResultId { get; set; }
        public int PollId { get; set; }
        public int PartyId { get; set; }
    }
}
