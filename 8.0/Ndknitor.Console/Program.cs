
using System.Diagnostics;
using System.Text;
using Ndknitor.System;

SCryptHash hasher = new SCryptHash { };
//PBKDF2Hash hasher = new PBKDF2Hash{};
byte[] password = Encoding.UTF8.GetBytes("123456");
Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();
byte[] hash = hasher.Hash(password);
stopwatch.Stop();
Console.WriteLine(stopwatch.ElapsedMilliseconds);
Console.WriteLine(BitConverter.ToString(hash).Replace("-", null));
// Console.WriteLine(hasher.Verify(password, hash));

