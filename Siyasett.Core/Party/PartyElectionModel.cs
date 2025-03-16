using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Party
{
    public class PartyElectionModel
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public int SecimId { get; set; }
        public int YskSecimId { get; set; }
        public string Adi { get; set; } = string.Empty;
        public int SecimTuruId { get; set; }
        public string SecimTuruAdi { get; set; } = string.Empty;
        public DateTime SecimTarihi { get; set; }
        public int AldigiOySayisi { get; set; }
        public int AldigiOyOrani { get; set; }
        public int ToplamOySayisi { get; set; }
        public string SonucBaslik { get; set; }

    }
}
