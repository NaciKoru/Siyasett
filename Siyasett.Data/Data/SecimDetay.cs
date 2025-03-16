using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimDetay
{
    public int Id { get; set; }

    public int? YskSecimId { get; set; }

    public int? SecimTuruId { get; set; }

    public int? YurtdisiSayi { get; set; }

    public int? TutanakSayi { get; set; }

    public int? BirlestirmeSayi { get; set; }

    public int? GumrukSayi { get; set; }

    public int? YskSecimDetayId { get; set; }

    public string? Adi { get; set; }

    public int? SecimId { get; set; }

    public int? ToplamSandik { get; set; }

    public int? AcilanSandik { get; set; }

    public int? AcilanSecmen { get; set; }

    public int? SecmenSayisi { get; set; }

    public int? OyKullanan { get; set; }

    public int? GecerliOy { get; set; }

    public int? ItirazliGecerliOy { get; set; }

    public int? ItirazsizGecerliOy { get; set; }

    public int? GecersizOy { get; set; }

    public int? AdaySayisi { get; set; }

    public int? BagimsizOy { get; set; }

    public virtual Secim? Secim { get; set; }

    public virtual ICollection<SecimIlceler> SecimIlcelers { get; } = new List<SecimIlceler>();

    public virtual ICollection<SecimIller> SecimIllers { get; } = new List<SecimIller>();

    public virtual ICollection<SecimPartiler> SecimPartilers { get; } = new List<SecimPartiler>();

    public virtual ICollection<SecimSandikSonuc> SecimSandikSonucs { get; } = new List<SecimSandikSonuc>();

    public virtual SecimTurleri? SecimTuru { get; set; }
}
