namespace Siyasett.Web.Models;

public class ElectionPartyResultModel
{
    public int? SecimId { get; set; }
    public int? PartyId { get; set; }
    public int? IlId { get; set; }
    public int? IlceId { get; set; }
    public int? Vote { get; set; }
    public int? YskSecimCevresiId { get; set; }
    public int? MuhtarlikId { get; set; }
    public int? IlListesiId { get; set; }
    public int? IlceListesiId { get; set; }

}


public class ElectionProvinceModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? RegionId { get; set; }
    public int? SecimIllerId { get; set; }
}