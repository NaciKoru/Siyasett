using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimTurleri
{
    public int Id { get; set; }

    public string? Adi { get; set; }

    public virtual ICollection<SecimDetay> SecimDetays { get; } = new List<SecimDetay>();
}
