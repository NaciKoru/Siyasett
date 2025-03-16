using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PeoplePhone
{
    public int Id { get; set; }

    public int PhoneId { get; set; }

    public int PeopleId { get; set; }

    public virtual Person People { get; set; } = null!;

    public virtual Phone Phone { get; set; } = null!;
}
