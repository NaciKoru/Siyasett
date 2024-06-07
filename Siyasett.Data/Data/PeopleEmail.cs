using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PeopleEmail
{
    public int Id { get; set; }

    public int PeopleId { get; set; }

    public int EmailId { get; set; }

    public virtual Email Email { get; set; } = null!;

    public virtual Person People { get; set; } = null!;
}
