using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Exceptions
{
    public class MissingOrderItemsException : Exception
    {
        public IList<int> _ids;

        public MissingOrderItemsException(IList<int> ids = null)
        {
            _ids = ids;            
        }
        public MissingOrderItemsException(string message)
            : base(message)
        {

        }
        public MissingOrderItemsException(string message, Exception inner)
            : base(message, inner)
        {

        }
        //public MissingOrderItemsException(string message, Exception inner, List<)
        //{

        //}
    }
}
