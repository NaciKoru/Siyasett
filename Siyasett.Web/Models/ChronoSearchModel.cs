namespace Siyasett.Web.Models
{
    public class ChronoSearchModel
    {
        public string EventDateStr { get; set; }
        public DateOnly EventDate { get; set; }

        public string KeywordTr { get; set; }
        public string KeywordEn { get; set; }
        public string DescriptionTr { get; set; }
        public string DescriptionEn { get; set; }
        public Guid UpdatedUserId { get; set; }
        public Guid CreatedUserId { get; set; }
        public string[] PartyNames { get; set; }
        public byte OrderBy { get; set; }
        public byte OrderByDesc { get; set; }
    }
}
