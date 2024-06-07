using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimIttifaklar
{
    public int Id { get; set; }

    public int? SecimDetayId { get; set; }

    public int? YskPartiId { get; set; }

    public int? YurtIciTolamOy { get; set; }

    public int? YurtDisiTolamOy { get; set; }

    public string? Adi { get; set; }

    public string? SonucBaslik { get; set; }
}
