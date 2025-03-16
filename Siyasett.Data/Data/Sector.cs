using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Sector
{
    public int Id { get; set; }

    public string? NameTr { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? CreatedUserId { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public bool? IsActive { get; set; }

    public string? NameEn { get; set; }

    public int? Weightiness { get; set; }

    public virtual ICollection<CompanySector> CompanySectors { get; } = new List<CompanySector>();

    public virtual ICollection<PartyText> PartyTexts { get; } = new List<PartyText>();

    public virtual ICollection<PeoplePositionSector> PeoplePositionSectors { get; } = new List<PeoplePositionSector>();

    public virtual ICollection<PeoplePosition> PeoplePositions { get; } = new List<PeoplePosition>();
}
