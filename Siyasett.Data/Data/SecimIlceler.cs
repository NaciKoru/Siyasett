using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimIlceler
{
    public int Id { get; set; }

    public int? IlId { get; set; }

    public int? IlceId { get; set; }

    public int? YskSecimId { get; set; }

    public int? YskSecimCevresiId { get; set; }

    public string? Adi { get; set; }

    public int? SecimDetayId { get; set; }

    public virtual SecimIller? Il { get; set; }

    public virtual SecimIlceListesi? Ilce { get; set; }

    public virtual SecimDetay? SecimDetay { get; set; }

    public virtual ICollection<SecimMuhtarliklar> SecimMuhtarliklars { get; } = new List<SecimMuhtarliklar>();
}
