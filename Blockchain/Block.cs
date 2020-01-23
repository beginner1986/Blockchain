using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    class Block
    {
        static int BlocksCount = 0;

        private readonly byte[] merkleHash;
        private readonly byte[] prevHash;
        private readonly DateTime mineTime;
        private readonly int blockNumber;
        private long randomNumber;
        private byte[] thisHash;

        public Block(List<byte[]> payments, byte[] prevHash, int j)
        {
            Random random = new Random();
            SHA256 sha256 = SHA256.Create();

            // class fields initialization
            blockNumber = BlocksCount++;
            merkleHash = MerkleTree(payments);
            this.prevHash = prevHash;
            mineTime = DateTime.Now;
            randomNumber = random.Next();

            // block as single byte array
            byte[] fullBlock = MakeFullBlock(merkleHash, prevHash, mineTime, blockNumber, randomNumber);
            thisHash = sha256.ComputeHash(fullBlock);
            int zerosCount = CountZeros(thisHash);

            // shuffle random number untill j initial zeros will appear in the beginning of hash
            while(zerosCount < j)
            {
                randomNumber = random.Next();
                fullBlock = MakeFullBlock(merkleHash, prevHash, mineTime, blockNumber, randomNumber);
                thisHash = sha256.ComputeHash(fullBlock);
                zerosCount = CountZeros(thisHash);
            }
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

        private int CountZeros(byte[] hash)
        {
            int result = 0;
            for(int i=0; i<hash.Length; i++)
            {
                if (hash[i] == 0)
                    result++;
                else
                    break;
            }

            return result;
        }

        private byte[] MakeFullBlock(byte[] merkleHash, byte[] prevHash,
        DateTime mineTime, int blockNumber, long randomNumber)
        {
            byte[] mineTimeBytes = Encoding.UTF8.GetBytes(mineTime.ToString());
            byte[] blockNumberBytes = BitConverter.GetBytes(blockNumber);
            byte[] randomNumberBytes = BitConverter.GetBytes(randomNumber);
            byte[] fullBlock =
                new byte[merkleHash.Length + prevHash.Length + mineTimeBytes.Length
                + blockNumberBytes.Length + randomNumberBytes.Length];

            byte[] result = new byte[merkleHash.Length + prevHash.Length + mineTimeBytes.Length
                + blockNumberBytes.Length + randomNumberBytes.Length];

            int position = 0;
            Array.Copy(merkleHash, 0, result, position, merkleHash.Length);
            position += merkleHash.Length;
            Array.Copy(prevHash, 0, result, position, prevHash.Length);
            position += prevHash.Length;
            Array.Copy(mineTimeBytes, 0, result, position, mineTimeBytes.Length);
            position += mineTimeBytes.Length;
            Array.Copy(blockNumberBytes, 0, result, position, blockNumberBytes.Length);
            position += blockNumberBytes.Length;
            Array.Copy(randomNumberBytes, 0, result, position, randomNumberBytes.Length);

            return result;
        }

        public override string ToString()
        {
            /*
                private readonly byte[] merkleHash;
                private readonly byte[] prevHash;
                private readonly DateTime mineTime;
                private readonly int blockNumber;
                private long randomNumber;
                private byte[] thisHash;
             */

            StringBuilder result = new StringBuilder();

            // block nuber
            result.Append("Block number: ");
            result.Append(blockNumber);

            // Merkle tree hash
            result.Append("\nTotal hash: ");
            for (int i = 0; i < merkleHash.Length; i++)
                result.Append(merkleHash[i].ToString("x2"));

            // previous block hash
            result.Append("\nPrevious hash: ");
            for (int i = 0; i < prevHash.Length; i++)
                result.Append(prevHash[i].ToString("x2"));

            // mining time
            result.Append("\nBlock mining time: ");
            result.Append(mineTime.ToString());

            // random number
            result.Append("\nRandom number: ");
            result.Append(randomNumber);

            // this block hash
            result.Append("\nCurrent block hash: ");
            for (int i = 0; i < thisHash.Length; i++)
                result.Append(thisHash[i].ToString("x2"));

            return result.ToString();
        }
    }
}
