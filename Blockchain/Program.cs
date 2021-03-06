﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Blockchain
{
    class Program
    {
        static void Main(string[] args)
        {
            // number of expected 0s in the beginning
            Console.Write("Podaj oczekiwaną ilość zer (j): ");
            if (!int.TryParse(Console.ReadLine(), out int j))
                j = 2;
            Console.WriteLine();

            SHA256 sha256 = SHA256.Create();
            Random random = new Random();

            // previous (random) block hash
            Console.WriteLine("Generowanie poprzedniego bloku...");
            byte[] initValue = new byte[256];
            random.NextBytes(new byte[256]);
            byte[] prevHash = sha256.ComputeHash(initValue);
            List<byte[]> payments = new List<byte[]>();

            // transactions generation
            Console.WriteLine("Generowanie transakcji...");
            for (byte i = 0; i < 3; i++)
            {
                byte[] payment = new byte[256];
                random.NextBytes(payment);
                payments.Add(sha256.ComputeHash(payment));
            }

            // generate the block
            Console.WriteLine("Tworzenie bloku...");
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Block block = new Block(payments, prevHash, j);
            timer.Stop();
            TimeSpan ts = timer.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("\nCzas obliczdeń: " + elapsedTime);
            Console.WriteLine();

            // print the generated blok
            Console.WriteLine(block.ToString());

            // hold the screen
            Console.ReadKey(true);
        }
    }
}
