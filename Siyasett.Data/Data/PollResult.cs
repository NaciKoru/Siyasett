using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PollResult
{
    public int Id { get; set; }

    public int PartyId { get; set; }

    public double Ratio { get; set; }

    public int PollId { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? CreatedUserId { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public virtual Party Party { get; set; } = null!;

    public virtual Poll Poll { get; set; } = null!;
}
