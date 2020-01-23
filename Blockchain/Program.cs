using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Blockchain
{
    class Program
    {
        static void Main(string[] args)
        {
            // input payments
            SHA256 sha256 = SHA256.Create();
            byte[] prevHash = sha256.ComputeHash(new byte[] { 9 });
            List<byte[]> payments = new List<byte[]>();
            // number of expected 0s in the beginning
            int j = 5;

            for (byte i = 0; i < 7; i++)
                payments.Add(sha256.ComputeHash(new byte[] { i }));

            Block block = new Block(payments, prevHash, j);
        }
    }
}
