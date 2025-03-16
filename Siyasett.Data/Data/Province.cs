using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Siyasett.Data.Data;

public partial class Province
{
    public MultiPolygon? Geom { get; set; }

    public int Id { get; set; }

    public string? Name { get; set; }

    public int CountryId { get; set; }
}
