using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class YynTurler
{
    public int Id { get; set; }

    public string Adi { get; set; } = null!;

    public virtual ICollection<YynKoseYazilari> YynKoseYazilaris { get; } = new List<YynKoseYazilari>();
}
