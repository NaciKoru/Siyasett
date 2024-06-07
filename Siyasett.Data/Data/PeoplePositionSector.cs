using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PeoplePositionSector
{
    public int Id { get; set; }

    public int PeoplePositionId { get; set; }

    public int SectorId { get; set; }

    public virtual PeoplePosition PeoplePosition { get; set; } = null!;

    public virtual Sector Sector { get; set; } = null!;
}
