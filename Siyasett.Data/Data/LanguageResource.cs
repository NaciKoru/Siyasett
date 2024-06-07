using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class LanguageResource
{
    public int Id { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    public virtual Language Language { get; set; } = null!;
}
