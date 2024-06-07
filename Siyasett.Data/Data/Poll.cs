using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Poll
{
    public int Id { get; set; }

    public int PollCompanyId { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public int? PollTypeId { get; set; }

    public int? PollKindId { get; set; }

    public int SampleSize { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? CreatedUserId { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public int? Day { get; set; }

    public virtual Company PollCompany { get; set; } = null!;

    public virtual PollKind? PollKind { get; set; }

    public virtual ICollection<PollResult> PollResults { get; } = new List<PollResult>();

    public virtual PollType? PollType { get; set; }
}
