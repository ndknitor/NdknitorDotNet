using System;
using System.Collections.Generic;

namespace Ndknitor.Console.Context;

public partial class Route
{
    public int RouteId { get; set; }

    public string From { get; set; }

    public string To { get; set; }

    public int BasePrice { get; set; }

    public ulong Deleted { get; set; }

    public virtual ICollection<Trip> Trip { get; set; } = new List<Trip>();
}
