using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimIlceListesi
{
    public int Id { get; set; }

    public int? IlId { get; set; }

    public string? Adi { get; set; }

    public virtual SecimIlListesi? Il { get; set; }

    public virtual ICollection<SecimIlceler> SecimIlcelers { get; } = new List<SecimIlceler>();
}
