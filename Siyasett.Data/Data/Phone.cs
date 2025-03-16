using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Phone
{
    public int Id { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? CreatedUserId { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public int PhoneTypeId { get; set; }

    public string? CountryCode { get; set; }

    public string? PrefixCode { get; set; }

    public int CommunicationTypeId { get; set; }

    public int CountryId { get; set; }

    public virtual CommunicationType CommunicationType { get; set; } = null!;

    public virtual ICollection<CompanyPhone> CompanyPhones { get; } = new List<CompanyPhone>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<PartyPhone> PartyPhones { get; } = new List<PartyPhone>();

    public virtual ICollection<PeoplePhone> PeoplePhones { get; } = new List<PeoplePhone>();

    public virtual PhoneType PhoneType { get; set; } = null!;
}
