using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Ilceler
{
    public string? Id { get; set; }

    public int? IlceId { get; set; }

    public string? IlceAdi { get; set; }

    public int? BeldeId { get; set; }

    public int? BirimId { get; set; }

    public int? IlId { get; set; }

    public string? IlAdi { get; set; }

    public int? SecimCevresiId { get; set; }

    public int? YerlesimYeriTuru { get; set; }

    public int? SecimDetayId { get; set; }

    public int? SecilecekAdaySayisi { get; set; }

    public int TekilId { get; set; }

    public int? SecimId { get; set; }

    public int? SecimTuruId { get; set; }

    public bool? Isok { get; set; }
}
