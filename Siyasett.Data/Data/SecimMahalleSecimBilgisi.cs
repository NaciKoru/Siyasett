using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimMahalleSecimBilgisi
{
    public int Id { get; set; }

    public int? MahalleId { get; set; }

    public int? SecimDetayId { get; set; }

    public int? MinSandikNo { get; set; }

    public int? MaxSandikNo { get; set; }
}
