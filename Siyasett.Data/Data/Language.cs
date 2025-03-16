using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Culture { get; set; } = null!;

    public virtual ICollection<LanguageResource> LanguageResources { get; } = new List<LanguageResource>();
}
