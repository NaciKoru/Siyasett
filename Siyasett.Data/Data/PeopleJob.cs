using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PeopleJob
{
    public int PeopleId { get; set; }

    public int JobId { get; set; }

    public int Id { get; set; }

    public virtual Job Job { get; set; } = null!;

    public virtual Person People { get; set; } = null!;
}
