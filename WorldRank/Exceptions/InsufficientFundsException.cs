using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank.Exceptions
{
    internal class InsufficientFundsException : WalletException
    {
        public InsufficientFundsException() : base("This wallet has insufficient funds.")
        {

        }
        public InsufficientFundsException(string message) : base(message)
        {

        }
    }
}
