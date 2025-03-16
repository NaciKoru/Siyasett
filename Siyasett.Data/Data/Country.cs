using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Country
{
    public string? NameTr { get; set; }

    public string? NameEn { get; set; }

    public string? PhoneCode { get; set; }

    public string? Iso2 { get; set; }

    public string? Iso3 { get; set; }

    public int? Sira { get; set; }

    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? CreatedUserId { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Address> Addresses { get; } = new List<Address>();

    public virtual ICollection<Phone> Phones { get; } = new List<Phone>();
}
