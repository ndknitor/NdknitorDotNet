using System;
using System.Collections.Generic;

namespace Ndknitor.Console.Context;

public partial class Seat
{
    public int SeatId { get; set; }

    public int BusId { get; set; }

    public int Price { get; set; }

    public ulong Deleted { get; set; }

    public string Name { get; set; }

    public virtual Bus Bus { get; set; }

    public virtual ICollection<Ticket> Ticket { get; set; } = new List<Ticket>();
}
