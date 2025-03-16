using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SandikSonucTutanaklari
{
    public int? Id { get; set; }

    public int? SecimId { get; set; }

    public int? SecimTuru { get; set; }

    public int? BirlestirmeId { get; set; }

    public string? FinishTime { get; set; }

    public int? FileType { get; set; }

    public int? PageNo { get; set; }

    public int? PageTotal { get; set; }

    public int? DetailId { get; set; }

    public int? UlkeId { get; set; }

    public int? SandikId { get; set; }
}
