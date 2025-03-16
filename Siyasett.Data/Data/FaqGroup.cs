using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class FaqGroup
{
    public int Id { get; set; }

    public string? NameTr { get; set; }

    public string? NameEn { get; set; }

    public virtual ICollection<Faq> Faqs { get; } = new List<Faq>();
}
