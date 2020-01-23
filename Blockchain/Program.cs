using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Blockchain
{
    class Program
    {
        static void Main(string[] args)
        {
            SHA256 sha256 = SHA256.Create();
            Random random = new Random();

            // previous (random) block hash
            Console.WriteLine("Generowanie poprzedniego bloku...");
            byte[] initValue = new byte[256];
            random.NextBytes(new byte[256]);
            byte[] prevHash = sha256.ComputeHash(initValue);
            List<byte[]> payments = new List<byte[]>();
            
            // number of expected 0s in the beginning
            int j = 2;

            // transactions generation
            Console.WriteLine("Generowanie transakcji...");
            for (byte i = 0; i < 70; i++)
            {
                byte[] payment = new byte[256];
                random.NextBytes(payment);
                payments.Add(sha256.ComputeHash(payment));
            }

            // generate the block
            Console.WriteLine("Tworzenie bloku...");
            Block block = new Block(payments, prevHash, j);

            // print the generated blosk
            block.ToString();
        }
    }
}
