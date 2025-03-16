using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimMuhtarliklar
{
    public int Id { get; set; }

    public int? IlceId { get; set; }

    public int? YskIlceId { get; set; }

    public int? SecimDetayId { get; set; }

    public int? MinSandikNo { get; set; }

    public int? MaxSandikNo { get; set; }

    public string? Adi { get; set; }

    public int? YskMuhtarlikId { get; set; }

    public virtual SecimIlceler? Ilce { get; set; }

    public virtual ICollection<SecimSandikSonuc> SecimSandikSonucs { get; } = new List<SecimSandikSonuc>();
}
