using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Attachment
{
    public int Id { get; set; }

    public string? NameTr { get; set; }

    public string? NameEn { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public string? ContentType { get; set; }

    public long FileSize { get; set; }

    public string? OriginalFileName { get; set; }

    public string? FileName { get; set; }

    public virtual ICollection<CompanyAttachment> CompanyAttachments { get; } = new List<CompanyAttachment>();

    public virtual ICollection<PartyAttachment> PartyAttachments { get; } = new List<PartyAttachment>();

    public virtual ICollection<PeopleAttachment> PeopleAttachments { get; } = new List<PeopleAttachment>();
}
