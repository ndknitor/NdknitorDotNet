
using System.Diagnostics;
using System.Text;
using Ndknitor.System;
using Newtonsoft.Json;
string s = JsonConvert.SerializeObject(new 
{
    alg = "HS256",
    typ = "JWT"
});
Console.WriteLine(Convert.ToBase64String(Encoding.UTF8.GetBytes(s)));

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

