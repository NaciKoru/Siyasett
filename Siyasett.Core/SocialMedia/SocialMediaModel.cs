using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.SocialMedia
{
    public class SocialMediaModel
    {
        public int Id { get; set; }
        public string? SocialAddress { get; set; }
        public int SocialTypeId { get; set; }
        public string? SocialTypeNameTr { get; set; }
        public string? SocialTypeNameEn { get; set; }

        public int ParentId { get; set; }
        public string Url { get; set; }

    }
}
