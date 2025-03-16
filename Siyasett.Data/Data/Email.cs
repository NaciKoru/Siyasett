using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Email
{
    public int Id { get; set; }

    public string EmailAddress { get; set; } = null!;

    public int CommunicationTypeId { get; set; }

    public virtual CommunicationType CommunicationType { get; set; } = null!;

    public virtual ICollection<CompanyEmail> CompanyEmails { get; } = new List<CompanyEmail>();

    public virtual ICollection<PartyEmail> PartyEmails { get; } = new List<PartyEmail>();

    public virtual ICollection<PeopleEmail> PeopleEmails { get; } = new List<PeopleEmail>();
}
