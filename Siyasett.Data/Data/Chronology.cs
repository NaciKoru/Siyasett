using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Chronology
{
    public int Id { get; set; }

    public DateOnly EventDate { get; set; }

    public string? KeywordsTr { get; set; }

    public string? KeywordsEn { get; set; }

    public string? DescriptionTr { get; set; }

    public string? DescriptionEn { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? UpdatedUserId { get; set; }

    public Guid? CreatedUserId { get; set; }

    public virtual ICollection<ChronologiesParty> ChronologiesParties { get; } = new List<ChronologiesParty>();
}
