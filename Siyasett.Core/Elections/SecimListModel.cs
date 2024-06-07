using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Elections
{
    public class SecimListModel
    {
        public int Id { get; set; }
        public int SecimDetayId { get; set; }
        public string Adi { get; set; }
        public bool Secili { get; set; }
    }
}
