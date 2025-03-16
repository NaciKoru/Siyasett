using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.PoliticalLibrary
{
    public class PoliticalLibrarySearchModel
    {
        public string KategoriAdi { get; set; }
        public string Adi { get; set; }
        public string YazarAdi { get; set; }
        public string YayinEviAdi { get; set; }
        public string DilAdi { get; set; }
        public byte OrderBy { get; set; }
        public byte OrderByDesc { get; set; }

    }
}
