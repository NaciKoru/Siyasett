using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SecimMuhtarlikSandiklar
{
    public int Id { get; set; }

    public int? SecimDetayId { get; set; }

    public int? SecimMuhtarlikId { get; set; }

    public int? YskIlceId { get; set; }

    public int? YskMuhtarlikId { get; set; }

    public int? SandikNo { get; set; }

    public bool? Isok { get; set; }
}
