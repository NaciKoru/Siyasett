using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.VideoLibrary
{
    public class VideoLibraryModel : BaseModel
    {
        public int Id { get; set; }
        public int? Session { get; set; }
        public string? Header { get; set; }
        public int? Language { get; set; }
        public string? VideoLink { get; set; }
    }
}
