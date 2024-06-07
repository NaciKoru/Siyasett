using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PartyPhone
{
    public int Id { get; set; }

    public int PhoneId { get; set; }

    public int PartyId { get; set; }

    public virtual Party Party { get; set; } = null!;

    public virtual Phone Phone { get; set; } = null!;
}
