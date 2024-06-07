using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class YynKitapKategoriler
{
    public int Id { get; set; }

    public int KitapId { get; set; }

    public int KategoriId { get; set; }

    public virtual YynKategoriler Kategori { get; set; } = null!;

    public virtual YynKitaplar Kitap { get; set; } = null!;
}
