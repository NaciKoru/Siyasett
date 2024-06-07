using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class YynKoseYazilariKategorileri
{
    public int Id { get; set; }

    public int KoseYazisiId { get; set; }

    public int KategoriId { get; set; }
}
