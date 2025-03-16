using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Currency
{
    public int Id { get; set; }

    public string? NameTr { get; set; }

    public DateTime Createdtime { get; set; }

    public DateTime Updatedtime { get; set; }

    public Guid? Createdby { get; set; }

    public Guid? Updatedby { get; set; }

    public bool? IsActive { get; set; }

    public string? AbbrTr { get; set; }

    public string? AbbrEn { get; set; }

    public string? NameEn { get; set; }

    public string? Symbol { get; set; }
}
