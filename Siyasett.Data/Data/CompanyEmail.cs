using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class CompanyEmail
{
    public int Id { get; set; }

    public int EmailId { get; set; }

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Email Email { get; set; } = null!;
}
