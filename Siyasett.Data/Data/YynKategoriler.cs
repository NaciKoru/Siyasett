using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class YynKategoriler
{
    public int Id { get; set; }

    public string Adi { get; set; } = null!;

    public virtual ICollection<YynKitapKategoriler> YynKitapKategorilers { get; } = new List<YynKitapKategoriler>();
}
