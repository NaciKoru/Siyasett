using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PeopleSocialMedium
{
    public int Id { get; set; }

    public int PeopleId { get; set; }

    public int SocialMediaId { get; set; }

    public virtual Person People { get; set; } = null!;

    public virtual SocialMedium SocialMedia { get; set; } = null!;
}
