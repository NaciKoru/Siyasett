using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Person
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? DateOfBirth { get; set; }

    public string? PlaceOfBirth { get; set; }

    public int GenderId { get; set; }

    public int EducationId { get; set; }

    public int? PartyId { get; set; }

    public int InstitutionTypeId { get; set; }

    public int PositionFieldId { get; set; }

    public string? CvTr { get; set; }

    public Guid? UserId { get; set; }

    public string? Photo { get; set; }

    public string? CvEn { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? CreatedUserId { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public bool IsActive { get; set; }

    public string? FirstNameNormalize { get; set; }

    public string? LastNameNormalize { get; set; }

    public int Views { get; set; }

    public virtual Education Education { get; set; } = null!;

    public virtual Gender Gender { get; set; } = null!;

    public virtual InstitutionType InstitutionType { get; set; } = null!;

    public virtual ICollection<Party> Parties { get; } = new List<Party>();

    public virtual ICollection<PeopleAddress> PeopleAddresses { get; } = new List<PeopleAddress>();

    public virtual ICollection<PeopleAttachment> PeopleAttachments { get; } = new List<PeopleAttachment>();

    public virtual ICollection<PeopleEmail> PeopleEmails { get; } = new List<PeopleEmail>();

    public virtual ICollection<PeopleJob> PeopleJobs { get; } = new List<PeopleJob>();

    public virtual ICollection<PeoplePhone> PeoplePhones { get; } = new List<PeoplePhone>();

    public virtual ICollection<PeoplePosition> PeoplePositions { get; } = new List<PeoplePosition>();

    public virtual ICollection<PeopleSocialMedium> PeopleSocialMedia { get; } = new List<PeopleSocialMedium>();

    public virtual PositionField PositionField { get; set; } = null!;

    public virtual AspNetUser? User { get; set; }

    public virtual ICollection<YynKoseYazilari> YynKoseYazilaris { get; } = new List<YynKoseYazilari>();
}
