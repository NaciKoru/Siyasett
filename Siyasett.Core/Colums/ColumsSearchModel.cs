using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Colums
{
    public class ColumsSearchModel
    {
        public byte OrderBy { get; set; }
        public byte OrderByDesc { get; set; }
        public DateTime? Date { get; set; }
        public string Header { get; set; }
        public string YazarAdSoyad { get; set; }
        public string TurAdi { get; set; }
        public string SirketAdi { get; set; }
        public int DilId { get; set; }
        public int ReadCount  { get; set; }
        public int YazarId { get; set; }
        public int SirketId { get; set; }
        public string ContextText { get; set; }
        public int Kategori { get; set; }
        public int TurId { get; set; }
        public int DateId { get; set; }
    }
}
