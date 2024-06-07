using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimSonucListesi
{
    public int? ToplamSandikSayisi { get; set; }

    public int? AcilanSandikSayisi { get; set; }

    public int? AcilanSecmenSayisi { get; set; }

    public int? SecmenSayisi { get; set; }

    public int? OyKullananSecmenSayisi { get; set; }

    public int? ItirazsizGecerliOySayisi { get; set; }

    public int? ItirazliGecerliOySayisi { get; set; }

    public int? GecerliOyToplami { get; set; }

    public int? GecersizOyToplami { get; set; }

    public int? BirlestirmeTutanagiIl { get; set; }

    public int? BirlestirmeTutanagiIlce { get; set; }

    public int? BirlestirmeTutanagiKurul { get; set; }

    public int? BirlestirmeTutanagiUlke { get; set; }

    public int? BirlestirmeTutanagiDisTemsilcilik { get; set; }

    public int? BirlestirmeTutanagiGumrukIlce { get; set; }

    public int? BirlestirmeTutanagiGumrukKurul { get; set; }

    public int? BirlestirmeTutanagiGumrukRumuz { get; set; }

    public int? BirlestirmeTutanagiUlkeler { get; set; }

    public int? BirlestirmeTutanagiGumrukler { get; set; }

    public int? BirlestirmeTutanagiTumdunya { get; set; }

    public int? SecilecekAdaySayisi { get; set; }

    public int TekilId { get; set; }

    public int? SecimDetayId { get; set; }

    public int? SecimTuruId { get; set; }
}
