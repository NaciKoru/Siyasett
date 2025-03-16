using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class CompanyAddress
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;
}
