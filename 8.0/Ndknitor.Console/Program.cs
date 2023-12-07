
using System.Text;
using Ndknitor.System;

PBKDF2Hasher hasher = new PBKDF2Hasher{

};
byte[] password = Encoding.UTF8.GetBytes("123456");
byte[] hash = hasher.Hash(password);
Console.WriteLine(BitConverter.ToString(hash).ToLower().Replace("-", null));
Console.WriteLine(hasher.Verify(password, hash));

