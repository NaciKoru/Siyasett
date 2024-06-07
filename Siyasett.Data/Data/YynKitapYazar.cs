using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class YynKitapYazar
{
    public int Id { get; set; }

    public int KitapId { get; set; }

    public int YazarId { get; set; }
}
