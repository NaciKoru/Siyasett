using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data
{
    public partial class SecimAlinmayanlar
    {
        public int Id { get; set; }
        public int? Sandikno { get; set; }
        public int? SecimDetayId { get; set; }
        public int? MahalleId { get; set; }
        public bool? Alindi { get; set; }
        public int? Ilceid { get; set; }
    }
}
