using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class VideoLibrary
{
    public int Id { get; set; }

    public int? Session { get; set; }

    public string? Head { get; set; }

    public int? Language { get; set; }

    public string? VideoLink { get; set; }
}
