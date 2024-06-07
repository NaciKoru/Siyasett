namespace Siyasett.Web.Models
{
    public class FeedBackModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Topic { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public string PageUrl { get; set; }

        public DateTime? SendTime { get; set; }

        public DateTime? AnswerTime { get; set; }
        public string Title { get; set; }

    }


}
