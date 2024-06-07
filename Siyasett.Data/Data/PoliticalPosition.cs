using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PoliticalPosition
{
    public int Id { get; set; }

    public string? NameTr { get; set; }

    public string? NameEn { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Party> Parties { get; } = new List<Party>();
}
