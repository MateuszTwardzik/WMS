using System;

namespace MagazynApp.Exceptions
{
    public class SocketNotFoundException  : Exception
    {
        public SocketNotFoundException()
        {
            
        }
        public SocketNotFoundException(string message)
            : base(message)
        {

        }
        public SocketNotFoundException(string message, Exception inner)
            : base(message, inner)
        {

        }
        
    }
}