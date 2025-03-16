using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimSonucBasliklari
{
    public int? SiraNo { get; set; }

    public string? Ad { get; set; }

    public string? ColumnName { get; set; }

    public int? SecimId { get; set; }

    public int? SecimTuruId { get; set; }

    public int? SecimCevresiId { get; set; }
}
