using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data
{
    public partial class PollParty
    {
        public int Id { get; set; }
        public string? PartyName { get; set; }
        public string? PartyRatio { get; set; }
        public int? PollId { get; set; }
    }
}
