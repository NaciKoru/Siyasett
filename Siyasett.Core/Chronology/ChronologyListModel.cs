using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Chronology
{
    public class ChronologyListModel:BaseModel
    {
        public string? EventDateStr { get; set; }
        public DateOnly EventDate { get; set; }
 
        public string? KeywordTr { get; set; }
        public string? KeywordEn { get; set; }
        public string? DescriptionTr { get; set; }
        public string? DescriptionEn { get; set; }
        public Guid UpdatedUserId { get; set; }
        public Guid CreatedUserId { get; set; }
        public string[] PartyNames { get; set; }

    }
}
