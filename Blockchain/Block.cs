using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    class Block
    {
        static int BlocksCount = 1;

        private readonly string merkleHash;
        private readonly string prevHash;
        private readonly DateTime mineTime;
        private readonly int blockNumber;
        private long randomNumber;
        private string thisHash;

        public Block(List<byte[]> payments, string prevHash, int j)
        {
            Random random = new Random();
            SHA256 sha256 = SHA256.Create();

            // class fields initialization
            blockNumber = BlocksCount++;
            merkleHash = MerkleTree(payments);
            this.prevHash = prevHash;
            mineTime = DateTime.Now;

            // block as single byte array
            string fullBlock = merkleHash + prevHash + mineTime.ToString() + blockNumber + randomNumber;
            int zerosCount = 0;

            // shuffle random number untill j initial zeros will appear in the beginning of hash
            while(zerosCount < j)
            {
                randomNumber = random.Next();
                string temp = fullBlock + randomNumber;
                thisHash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(temp)));
                zerosCount = CountZeros(thisHash);
            }
        }

        private string MerkleTree(List<byte[]> payments)
        {
            List<byte[]> hashes = new List<byte[]>();
            SHA256 sha256 = SHA256.Create();

            // compute hash of every single payment
            foreach(byte[] payment in payments)
                hashes.Add(sha256.ComputeHash(payment));

            // main loop
            while(hashes.Count > 1)
            {
                // next level of the tree
                List<byte[]> temp = new List<byte[]>();
             
                for(int i=0; i<hashes.Count; i+=2)
                {
                    // copy next hash or if it doesn;t exist take 0
                    byte[] next;
                    if (i + 1 < hashes.Count)
                        next = hashes[i + 1];
                    else
                        next = new byte[1] { 0 };

                    // two merged two censecutive hashes
                    byte[] merged = new byte[hashes[i].Length + next.Length];
                    Array.Copy(hashes[i], 0, merged, 0, hashes[i].Length);
                    Array.Copy(next, 0, merged, 0, next.Length);

                    // calculate merged hash
                    byte[] hash = sha256.ComputeHash(merged);
                    temp.Add(hash);
                }

                hashes = temp;
            }

            return BitConverter.ToString(hashes[0]);
        }

        private int CountZeros(string hash)
        {
            int result = 0;
            foreach(char c in hash)
            {
                if (c == '0')
                    result++;
                else
                    break;
            }

            /*
            Console.Write($"Zeros: {result} | ");
            foreach (byte b in hash)
                Console.Write($"{b.ToString()}");
            Console.WriteLine();
             */

            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            // block nuber
            result.Append("Numer bloku: ");
            result.Append(blockNumber);

            // Merkle tree hash
            result.Append("\nSkrót wszystkich transakcji: ");
            for (int i = 0; i < merkleHash.Length; i++)
                result.Append(merkleHash.Replace("-", ""));

            // previous block hash
            result.Append("\nSkrót z poprzedniego bloku: ");
            for (int i = 0; i < prevHash.Length; i++)
                result.Append(prevHash.Replace("-", ""));

            // mining time
            result.Append("\nCzas wykopania bloku: ");
            result.Append(mineTime.ToString());

            // random number
            result.Append("\nLiczba losowa: ");
            result.Append(randomNumber);

            // this block hash
            result.Append("\nSkrót bieżącego bloku: ");
            for (int i = 0; i < thisHash.Length; i++)
                result.Append(thisHash.Replace("-", ""));

            return result.ToString();
        }
    }
}
