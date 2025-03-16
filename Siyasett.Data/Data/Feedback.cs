using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class Feedback
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Lastname { get; set; }

    public string? Email { get; set; }

    public string? Topic { get; set; }

    public string? Message { get; set; }

    public string? Answer { get; set; }

    public string? PageLink { get; set; }

    public DateTime? SendTime { get; set; }

    public DateTime? AnswerTime { get; set; }

    public string? Title { get; set; }
}
