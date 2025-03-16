using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PartyText
{
    public int Id { get; set; }

    public int Partyid { get; set; }

    public string? Text { get; set; }

    public int SectorId { get; set; }

    public virtual Party Party { get; set; } = null!;

    public virtual Sector Sector { get; set; } = null!;
}
