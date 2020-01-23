using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    class Block
    {
        private int blockNumber;
        private byte[] merkleHash;
        private byte[] prevHash;
        private DateTime mineTime;
        private long randomNumber;
        private byte[] thisHash;
        private int j;

        public Block(List<byte[]> payments, byte[] prevHash, int j)
        {
            // TODO

            /*
            // Merkle tree test
            byte[] hash = MerkleTree(payments);
            for (int i = 0; i < hash.Length; i++)
                Console.Write($"{hash[i]}");
             */ 
        }

        private byte[] MerkleTree(List<byte[]> payments)
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

            return hashes[0];
        }
    }
}
