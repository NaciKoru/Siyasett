using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PeoplePosition
{
    public int Id { get; set; }

    public int PeopleId { get; set; }

    public int InstitutionTypeId { get; set; }

    public string? InstitutionName { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? PartyId { get; set; }

    public int PositionId { get; set; }

    public string? Period { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public short? StartDay { get; set; }

    public short? StartMonth { get; set; }

    public short? StartYear { get; set; }

    public short? EndDay { get; set; }

    public short? EndMonth { get; set; }

    public short? EndYear { get; set; }

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    public int? SectorId { get; set; }

    public virtual InstitutionType InstitutionType { get; set; } = null!;

    public virtual Party? Party { get; set; }

    public virtual Person People { get; set; } = null!;

    public virtual ICollection<PeoplePositionSector> PeoplePositionSectors { get; } = new List<PeoplePositionSector>();

    public virtual Position Position { get; set; } = null!;

    public virtual Sector? Sector { get; set; }
}
