using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data
{
    public partial class CompanyAttacment
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int AttacmentId { get; set; }

        public virtual Attachment Attacment { get; set; } = null!;
        public virtual Company Company { get; set; } = null!;
    }
}
