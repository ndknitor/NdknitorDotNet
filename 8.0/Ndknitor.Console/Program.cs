using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using Ndknitor.Console.Context;
using Ndknitor.EFCore;
using Ndknitor.System;
BenchmarkRunner.Run<SeatBenchmark>();

public class SeatBenchmark
{
    [Benchmark]
    public void MapOffsetIdWithoutMax()
    {
        var context = new EtdbContext(new DbContextOptions<EtdbContext>());
        IEnumerable<Seat> seats = new List<Seat>
        {
            new Seat
            {
                BusId = 2,
                Deleted = 0,
                Name = "asdasd",
                Price = 123123
            },
            new Seat
            {
                BusId = 2,
                Deleted = 0,
                Name = "asdasd",
                Price = 123123
            }
        };
        seats.MapIncreasement((s, o) => s.SeatId = o, context.Seat.Max(s => s.SeatId));
        Console.WriteLine(seats.ToJson());
    }
    [Benchmark]
    public void MapOffsetIdWithMax()
    {
        var context = new EtdbContext(new DbContextOptions<EtdbContext>());
        IEnumerable<Seat> seats = new List<Seat>
        {
            new Seat
            {
                BusId = 2,
                Deleted = 0,
                Name = "asdasd",
                Price = 123123
            },
            new Seat
            {
                BusId = 2,
                Deleted = 0,
                Name = "asdasd",
                Price = 123123
            }
        };
        int offsetId = context.Seat.Max(s => s.SeatId) + 1;
        foreach (var item in seats)
        {
            item.SeatId = offsetId++;
        }
        Console.WriteLine(seats.ToJson());
    }
}

// SCryptHash hasher = new SCryptHash { };
// //PBKDF2Hash hasher = new PBKDF2Hash{};
// byte[] password = Encoding.UTF8.GetBytes("123456");
// Stopwatch stopwatch = new Stopwatch();
// stopwatch.Start();
// byte[] hash = hasher.Hash(password);
// stopwatch.Stop();
// Console.WriteLine(stopwatch.ElapsedMilliseconds);
// Console.WriteLine(BitConverter.ToString(hash).Replace("-", null));
// // Console.WriteLine(hasher.Verify(password, hash));

