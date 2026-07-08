using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank.Exceptions
{
    internal class BlockedException : WalletException
    {
        public BlockedException(string message) : base(message)
        {

        }
        public BlockedException() : base("This wallet is blocked")
        {

        }
    }
}
