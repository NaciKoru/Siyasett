using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data
{
    public partial class PartyAttacment
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public int AttacmentId { get; set; }

        public virtual Attachment Attacment { get; set; } = null!;
        public virtual Party Party { get; set; } = null!;
    }
}
