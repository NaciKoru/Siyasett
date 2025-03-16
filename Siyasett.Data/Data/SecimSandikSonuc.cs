using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimSandikSonuc
{
    public int Id { get; set; }

    public int? SecimDetayId { get; set; }

    public int? YskMuhtarlikId { get; set; }

    public int? PartiId { get; set; }

    public int? SandikNo { get; set; }

    public int? OySayisi { get; set; }

    public int? MuhtarlikId { get; set; }

    public virtual SecimMuhtarliklar? Muhtarlik { get; set; }

    public virtual Party? Parti { get; set; }

    public virtual SecimDetay? SecimDetay { get; set; }
}
