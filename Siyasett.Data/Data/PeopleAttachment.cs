using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PeopleAttachment
{
    public int Id { get; set; }

    public int PeopleId { get; set; }

    public int AttachmentId { get; set; }

    public virtual Attachment Attachment { get; set; } = null!;

    public virtual Person People { get; set; } = null!;
}
