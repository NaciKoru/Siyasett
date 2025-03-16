using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Sonuclar
{
    public int? SecimSandikSonuId { get; set; }

    public int? SecimDetayId { get; set; }

    public int? IlId { get; set; }

    public int? SecimIllerId { get; set; }

    public int? SecimIlcelerId { get; set; }

    public int? SecimIlceListesiId { get; set; }

    public int? SecimMuhtarliklarId { get; set; }

    public int? MinSandikNo { get; set; }

    public int? MaxSandikNo { get; set; }

    public int? SandikNo { get; set; }

    public int? PartiId { get; set; }

    public int? OySayisi { get; set; }

    public int? YskSecimCevresiId { get; set; }
}
