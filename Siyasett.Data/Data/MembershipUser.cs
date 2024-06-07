using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class MembershipUser
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime MshipStartdate { get; set; }

    public DateTime? MshipEnddate { get; set; }

    public int? MshipType { get; set; }
}
