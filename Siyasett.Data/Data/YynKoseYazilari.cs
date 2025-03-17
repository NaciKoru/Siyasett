using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class YynKoseYazilari
{
    public int Id { get; set; }

    public int DilId { get; set; }

    public int TurId { get; set; }

    public int YazarId { get; set; }
    public int SirketId { get; set; }
    public string Baslik { get; set; } = null!;
    public string? Metin { get; set; }
    public string? MetinOzet { get; set; } // New field for summary
    public DateTime Tarih { get; set; }
    public string? Url { get; set; }
    public int OkunmaSayisi { get; set; }
    public string? Keywords { get; set; }
    public string? BaslikEn { get; set; }
    public string? MetinEn { get; set; }
    public string? MetinOzetEn { get; set; } // New field for English summary

    public virtual YynDiller Dil { get; set; } = null!;

    public virtual Company Sirket { get; set; } = null!;

    public virtual YynTurler Tur { get; set; } = null!;

    public virtual Person Yazar { get; set; } = null!;
}
