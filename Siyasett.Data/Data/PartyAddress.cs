using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PartyAddress
{
    public int Id { get; set; }

    public int PartyId { get; set; }

    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual Party Party { get; set; } = null!;
}
