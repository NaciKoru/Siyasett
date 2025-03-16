using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimDetayListesi
{
    public int? SecimId { get; set; }

    public string? AskiBitisTarihi { get; set; }

    public int? YurtdisiSayi { get; set; }

    public int? BirlestirmeSayi { get; set; }

    public int? GumrukSayi { get; set; }

    public DateTime? AskiBaslangicTarihi { get; set; }

    public int? SecimTuruKodu { get; set; }

    public string? SecimTuruAdi { get; set; }

    public int SecimDetayId { get; set; }

    public int? TutanakSayi { get; set; }
}
