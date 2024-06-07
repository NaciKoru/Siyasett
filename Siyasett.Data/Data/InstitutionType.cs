using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class InstitutionType
{
    public int Id { get; set; }

    public string? NameTr { get; set; }

    public string? NameEn { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? CreatedUserId { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Person> People { get; } = new List<Person>();

    public virtual ICollection<PeoplePosition> PeoplePositions { get; } = new List<PeoplePosition>();
}
