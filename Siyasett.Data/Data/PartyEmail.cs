using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PartyEmail
{
    public int Id { get; set; }

    public int EmailId { get; set; }

    public int PartyId { get; set; }

    public virtual Email Email { get; set; } = null!;

    public virtual Party Party { get; set; } = null!;
}
