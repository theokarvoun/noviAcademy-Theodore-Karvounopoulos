using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank.Exceptions
{
    internal class WalletException : Exception
    {
        public WalletException() : base("Wallet Exception")
        {

        }
        public WalletException(string message) : base(message)
        {

        }
    }
}
