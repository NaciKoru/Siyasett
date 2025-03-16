using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Party
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ShortName { get; set; }

    /// <summary>
    /// kuruluş tarihi
    /// </summary>
    public string? Dof { get; set; }

    public bool Active { get; set; }

    public bool ParticipateElection { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? CreatedUserId { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public int? YskId { get; set; }

    public int? LeaderPeopleId { get; set; }

    public string? Logo { get; set; }

    public int? MemberCount { get; set; }

    public string? WebSite { get; set; }

    public bool Closed { get; set; }

    public int? Parliamenteries { get; set; }

    public string? NameEn { get; set; }

    public int? PoliticalPositionId { get; set; }

    public virtual Person? LeaderPeople { get; set; }

    public virtual ICollection<PartyAddress> PartyAddresses { get; } = new List<PartyAddress>();

    public virtual ICollection<PartyAttachment> PartyAttachments { get; } = new List<PartyAttachment>();

    public virtual ICollection<PartyEmail> PartyEmails { get; } = new List<PartyEmail>();

    public virtual ICollection<PartyIdeology> PartyIdeologies { get; } = new List<PartyIdeology>();

    public virtual ICollection<PartyPhone> PartyPhones { get; } = new List<PartyPhone>();

    public virtual ICollection<PartySocialParty> PartySocialParties { get; } = new List<PartySocialParty>();

    public virtual ICollection<PartyText> PartyTexts { get; } = new List<PartyText>();

    public virtual ICollection<PeoplePosition> PeoplePositions { get; } = new List<PeoplePosition>();

    public virtual PoliticalPosition? PoliticalPosition { get; set; }

    public virtual ICollection<PollResult> PollResults { get; } = new List<PollResult>();

    public virtual ICollection<SecimPartiler> SecimPartilers { get; } = new List<SecimPartiler>();

    public virtual ICollection<SecimSandikSonuc> SecimSandikSonucs { get; } = new List<SecimSandikSonuc>();
}
