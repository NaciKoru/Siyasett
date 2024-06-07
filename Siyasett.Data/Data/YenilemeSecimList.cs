using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class YenilemeSecimList
{
    public int? SecimId { get; set; }

    public string? SecimAdi { get; set; }

    public DateTime? SecimTarihi { get; set; }

    public int? SecimTuruKodu { get; set; }

    public int? SecimCevresiId { get; set; }

    public int? YerlesimYeriTuru { get; set; }

    public int? SecimDetayId { get; set; }

    public string? SecimCevresiAdi { get; set; }

    public int? YerlesimYeriId { get; set; }
}
