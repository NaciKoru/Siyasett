using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data
{
    public partial class PeoplePositionsOld
    {
        public int PeopleId { get; set; }
        public int PositionId { get; set; }
        public int Id { get; set; }

        public virtual Person People { get; set; } = null!;
        public virtual Position Position { get; set; } = null!;
    }
}
