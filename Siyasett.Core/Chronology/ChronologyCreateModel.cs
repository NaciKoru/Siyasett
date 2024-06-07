using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Chronology
{
    public class ChronologyCreateModel:BaseModel
    {
        public string EventDateStr { get; set; }
  
        public string? KeywordTr { get; set; }
        public string? KeywordEn { get; set; }
        public string? DescriptionTr { get; set; }
        public string? DescriptionEn { get; set; }
        public int[] PartyIds { get; set; }


    }
}
