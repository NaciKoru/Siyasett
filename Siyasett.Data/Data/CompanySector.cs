using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class CompanySector
{
    public int CompanyId { get; set; }

    public int SectorId { get; set; }

    public int Id { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Sector Sector { get; set; } = null!;
}
