using System;

namespace MagazynApp.Exceptions
{
    public class SupplyIsCompletedException : Exception
    {
        public SupplyIsCompletedException()
        {
        }

        public SupplyIsCompletedException(string message)
            : base(message)
        {
        }

        public SupplyIsCompletedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}