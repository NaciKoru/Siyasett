using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class PartyIdeology
{
    public int Id { get; set; }

    public int PartyId { get; set; }

    public int PoliticialIdeologyId { get; set; }

    public int Value { get; set; }

    public virtual Party Party { get; set; } = null!;

    public virtual PoliticalIdeology PoliticialIdeology { get; set; } = null!;
}
