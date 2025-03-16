using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimIlListesi
{
    public int Id { get; set; }

    public string? Adi { get; set; }

    public virtual ICollection<SecimIlceListesi> SecimIlceListesis { get; } = new List<SecimIlceListesi>();

    public virtual ICollection<SecimIller> SecimIllers { get; } = new List<SecimIller>();
}
