using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PartySocialParty
{
    public int Id { get; set; }

    public int PartyId { get; set; }

    public int SocialMediaId { get; set; }

    public virtual Party Party { get; set; } = null!;

    public virtual SocialMedium SocialMedia { get; set; } = null!;
}
