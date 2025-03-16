using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Address
{
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? CreatedUserId { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public string? NameTr { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? District { get; set; }

    public string? Province { get; set; }

    public int CountryId { get; set; }

    public string? PostalCode { get; set; }

    public int AddressTypeId { get; set; }

    public string? NameEn { get; set; }

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    public virtual AddressType AddressType { get; set; } = null!;

    public virtual ICollection<CompanyAddress> CompanyAddresses { get; } = new List<CompanyAddress>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<PartyAddress> PartyAddresses { get; } = new List<PartyAddress>();

    public virtual ICollection<PeopleAddress> PeopleAddresses { get; } = new List<PeopleAddress>();
}
