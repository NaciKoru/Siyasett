using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SocialMedium
{
    public int Id { get; set; }

    public string SocialAddress { get; set; } = null!;

    public int SocialTypeId { get; set; }

    public virtual ICollection<CompanySocialMedia> CompanySocialMedia { get; } = new List<CompanySocialMedia>();

    public virtual ICollection<PartySocialParty> PartySocialParties { get; } = new List<PartySocialParty>();

    public virtual ICollection<PeopleSocialMedium> PeopleSocialMedia { get; } = new List<PeopleSocialMedium>();

    public virtual SocialType SocialType { get; set; } = null!;
}
