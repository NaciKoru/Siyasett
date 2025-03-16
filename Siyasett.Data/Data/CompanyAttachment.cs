using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class CompanyAttachment
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int AttachmentId { get; set; }

    public virtual Attachment Attachment { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;
}
