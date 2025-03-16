using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Attachments
{
    public class AttachmentModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int AttachmentId { get; set; }
        public string NameTr { get; set; }
        public string NameEn { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
