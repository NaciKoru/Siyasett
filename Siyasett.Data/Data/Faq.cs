using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Faq
{
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public string? QuestionTr { get; set; }

    public string? QuestionEn { get; set; }

    public string? AnswerTr { get; set; }

    public string? AnswerEn { get; set; }

    public int FaqGroupId { get; set; }

    public virtual FaqGroup FaqGroup { get; set; } = null!;
}
