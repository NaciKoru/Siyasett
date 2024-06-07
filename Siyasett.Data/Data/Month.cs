using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data
{
    public partial class Month
    {
        public int Id { get; set; }
        public string? NameTr { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }
        public string? NameEn { get; set; }
    }
}
