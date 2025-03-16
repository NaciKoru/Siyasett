using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PartyAttachment
{
    public int Id { get; set; }

    public int PartyId { get; set; }

    public int AttachmentId { get; set; }

    public virtual Attachment Attachment { get; set; } = null!;

    public virtual Party Party { get; set; } = null!;
}
