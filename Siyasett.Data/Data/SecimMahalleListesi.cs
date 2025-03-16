using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimMahalleListesi
{
    public int Id { get; set; }

    public int? IlceId { get; set; }

    public string? Adi { get; set; }

    public int? YskMuhtarlikId { get; set; }
}
