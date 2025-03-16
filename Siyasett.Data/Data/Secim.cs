using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Secim
{
    public int Id { get; set; }

    public int? YskSecimId { get; set; }

    public int? TekilId { get; set; }

    public int? YurtdisiSayi { get; set; }

    public DateTime? SecimTarihi { get; set; }

    public DateTime? BaslangicTarihi { get; set; }

    public DateTime? AdaylikKesinlesmeTarihi { get; set; }

    public DateTime? SecmenSonHareketTarihi { get; set; }

    public DateTime? ResmiGazeteTarihi { get; set; }

    public bool? Arasecim { get; set; }

    public bool? Yenilemesecimi { get; set; }

    public int? YenilenenSecimId { get; set; }

    public int? YskYenilenenSecimId { get; set; }

    public string? Adi { get; set; }

    public int? SecimTuruId { get; set; }

    public virtual ICollection<SecimDetay> SecimDetays { get; } = new List<SecimDetay>();
}
