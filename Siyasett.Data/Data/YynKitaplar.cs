using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class YynKitaplar
{
    public int Id { get; set; }

    public string Adi { get; set; } = null!;

    public int? YayinEviId { get; set; }

    public int DilId { get; set; }

    public int Yili { get; set; }

    public string? EKitapUrl { get; set; }

    public string? Image { get; set; }

    public string? Aciklama { get; set; }

    public virtual YynDiller Dil { get; set; } = null!;

    public virtual ICollection<YynKitapKategoriler> YynKitapKategorilers { get; } = new List<YynKitapKategoriler>();
}
