using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? CreatedUserId { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public string? Logo { get; set; }

    public bool Active { get; set; }

    public int? CompanyLeaderId { get; set; }

    /// <summary>
    /// kuruluş tarihi
    /// </summary>
    public string? Dof { get; set; }

    public string? WebAdress { get; set; }

    public virtual ICollection<CompanyAddress> CompanyAddresses { get; } = new List<CompanyAddress>();

    public virtual ICollection<CompanyAttachment> CompanyAttachments { get; } = new List<CompanyAttachment>();

    public virtual ICollection<CompanyEmail> CompanyEmails { get; } = new List<CompanyEmail>();

    public virtual ICollection<CompanyPhone> CompanyPhones { get; } = new List<CompanyPhone>();

    public virtual ICollection<CompanySector> CompanySectors { get; } = new List<CompanySector>();

    public virtual ICollection<CompanySocialMedia> CompanySocialMedia { get; } = new List<CompanySocialMedia>();

    public virtual ICollection<Poll> Polls { get; } = new List<Poll>();

    public virtual ICollection<YynKoseYazilari> YynKoseYazilaris { get; } = new List<YynKoseYazilari>();
}
