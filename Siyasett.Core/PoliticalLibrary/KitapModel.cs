using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.PoliticalLibrary
{
    public class KitapModel
    {
        public int Id { get; set; }
        public string Adi { get; set; }
        public int? YayinEviId { get; set; }
        public int DilId { get; set; }
        public int Yili { get; set; }
        public string? EkitapUrl { get; set; }
        public string? Image { get; set; }
        public string? Aciklama { get; set; }
        public int[] KategoriId { get; set; }
        public int[] Yazari { get; set; }
        public string? YazarAdi  { get; set; }
        public string? YayinEviAdi { get; set; }
        public string? KategoriAdi { get; set; }
        public string? DilAdi { get; set; }
    }
}
