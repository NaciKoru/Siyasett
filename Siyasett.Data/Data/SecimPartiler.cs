using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimPartiler
{
    public int Id { get; set; }

    public int? PartiId { get; set; }

    public int? SecimDetayId { get; set; }

    public int? YskPartiId { get; set; }

    public int? YurtIciToplamOy { get; set; }

    public int? YurtDisiToplamOy { get; set; }

    public string? SonucBaslik { get; set; }

    public virtual Party? Parti { get; set; }

    public virtual SecimDetay? SecimDetay { get; set; }
}
