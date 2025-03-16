using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PeopleAddress
{
    public int Id { get; set; }

    public int PeopleId { get; set; }

    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual Person People { get; set; } = null!;
}
