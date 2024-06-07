using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class CompanyPhone
{
    public int Id { get; set; }

    public int PhoneId { get; set; }

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Phone Phone { get; set; } = null!;
}
