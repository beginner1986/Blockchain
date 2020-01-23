using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
{
    class Block
    {
        int blockNumber;
        byte[] merkleHash;
        byte[] prevHash;
        DateTime mineTime;
        long randomNumber;
        byte[] thisHash;
        int j;

        Block(List<byte[]> payments, byte[] prevHash, int j)
        {

        }
    }
}
