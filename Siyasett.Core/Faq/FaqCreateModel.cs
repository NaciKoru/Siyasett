using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Faq
{
    public class FaqCreateModel:BaseModel
    {
        public string? FaqQuestionTr { get; set; }
        public string? FaqQuestionEn { get; set; }
        public string? FaqAnswerTr { get; set; }
        public string? FaqAnswerEn { get; set; }
        public int FaqGroupId { get; set; }

    }
}
