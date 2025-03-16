using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Colums
{
    public class KoseYazilariKategoriModel
    {
        public int Id { get; set; }
        public int KoseYazisiId { get; set; }
        public int KategoriId { get; set; }
    }
}
