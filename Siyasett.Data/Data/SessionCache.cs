using System;
using System.Collections.Generic;

namespace Siyasett.Data.Data;

public partial class SessionCache
{
    public string Id { get; set; } = null!;

    public byte[]? Value { get; set; }

    public DateTime? ExpiresAtTime { get; set; }

    public double? SlidingExpirationInSeconds { get; set; }

    public DateTime? AbsoluteExpiration { get; set; }
}
