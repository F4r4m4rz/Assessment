using Assessment.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.Data.Exceptions
{
    public class EntryNotFoundException : Exception
    {
        public EntryNotFoundException() : base()
        {

        }

        public EntryNotFoundException(string message) : base(message)
        {

        }
    }

    public class CheckOutFailedException : Exception
    {
        public CheckOutFailedException() : base()
        {

        }

        public CheckOutFailedException(string message) : base(message)
        {

        }

        public CheckOutFailedException(string message, ShoppingCard shoppingCard) : base(message)
        {
            ShoppingCard = shoppingCard;
        }

        public ShoppingCard ShoppingCard { get; }
    }
}
