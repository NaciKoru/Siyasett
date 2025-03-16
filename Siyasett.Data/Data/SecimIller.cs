using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimIller
{
    public int Id { get; set; }

    public int? IlId { get; set; }

    public int? YskSecimId { get; set; }

    public int? YskSecimDetayId { get; set; }

    public int? YskSecimCevresiId { get; set; }

    public int? AdaySayisi { get; set; }

    public string? Adi { get; set; }

    public int? SecimDetayId { get; set; }

    public virtual SecimIlListesi? Il { get; set; }

    public virtual SecimDetay? SecimDetay { get; set; }

    public virtual ICollection<SecimIlceler> SecimIlcelers { get; } = new List<SecimIlceler>();
}
