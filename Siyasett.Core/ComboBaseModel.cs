using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core
{
    public partial class ComboBaseModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string? NameTr { get; set; }
        public string? NameEn { get; set; }
        public string UrlPattern { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Weightiness { get; set; }
    }
}
