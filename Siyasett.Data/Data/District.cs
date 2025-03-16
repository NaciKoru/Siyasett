using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Siyasett.Data.Data;

public partial class District
{
    public int Id { get; set; }

    public MultiPolygon? Geom { get; set; }

    public int ProvinceId { get; set; }

    public string? Name { get; set; }
}
