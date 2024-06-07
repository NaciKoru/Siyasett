using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class CompanySocialMedia
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int SocialMediaId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual SocialMedium SocialMedia { get; set; } = null!;
}
