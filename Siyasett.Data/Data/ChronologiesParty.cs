using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class ChronologiesParty
{
    public int Id { get; set; }

    public int Chronologyid { get; set; }

    public int Partyid { get; set; }

    public virtual Chronology Chronology { get; set; } = null!;
}
