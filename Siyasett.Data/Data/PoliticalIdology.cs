using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PoliticalIdology
{
    public int Id { get; set; }

    public string? NameTr { get; set; }

    public string? NameEn { get; set; }

    public bool? IsActive { get; set; }
}
