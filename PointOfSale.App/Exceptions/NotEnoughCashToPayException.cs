using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.App.Exceptions
{
    public class NotEnoughCashToPayException : Exception
    {
        public NotEnoughCashToPayException()
        {
        }

        public NotEnoughCashToPayException(string message)
            : base(message)
        {
        }

        public NotEnoughCashToPayException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
