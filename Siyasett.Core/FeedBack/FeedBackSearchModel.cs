using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.FeedBack
{
    public class FeedBackSearchModel
    {
        public string BildirimTarihi { get; set; }
        public string Adi { get; set; }
        public string Soyad { get; set; }
        public string Konu { get; set; }
        public string Bildirim { get; set; }
        public string Cevap { get; set; }
        public string CevapTarih { get; set; }

        public byte OrderBy { get; set; }
        public byte OrderByDesc { get; set; }

    }
}
