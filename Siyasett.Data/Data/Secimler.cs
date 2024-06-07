using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Secimler
{
    public int? SecimId { get; set; }

    public int? YurtdisiSayi { get; set; }

    public int? BirimId { get; set; }

    public string? SecimAdi { get; set; }

    public DateTime? SecimTarihi { get; set; }

    public DateTime? SecimBaslangicTarihi { get; set; }

    public DateTime? AdaylikKesinlesmeTarihi { get; set; }

    public DateTime? SecmenSonHareketTarihi { get; set; }

    public int? AraSecim { get; set; }

    public int? YenilemeSecimi { get; set; }

    public int? YenilenenSecimId { get; set; }

    public DateTime? YenilemeVeriOlusturmaTarihi { get; set; }

    public string? YenilemeVerisiOlusturanPers { get; set; }

    public DateTime? ResmiGazeteTarihi { get; set; }

    public string? SecimTipi { get; set; }

    public DateTime? SecmenDegisiklikListeTarihi { get; set; }

    public int? SecimDetaySayi { get; set; }

    public int TekilId { get; set; }
}
