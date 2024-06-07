using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimMuhtarlikOzet
{
    public int? IlId { get; set; }

    public int? IlceId { get; set; }

    public int? SecimId { get; set; }

    public int? SecimCevresiId { get; set; }

    public int? YskMuhtarlikId { get; set; }

    public long? Bagimsiz { get; set; }

    public long? Secmen { get; set; }

    public long? OyKullanan { get; set; }

    public long? Gecersiz { get; set; }

    public long? Gecerli { get; set; }

    public int? MuhtarlikId { get; set; }

    public int? SecimIllerId { get; set; }

    public int? SecimIlcelerId { get; set; }

    public int? SecimDetayId { get; set; }
}
