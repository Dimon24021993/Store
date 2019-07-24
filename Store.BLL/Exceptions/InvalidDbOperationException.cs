using System;

namespace Store.BLL.Exceptions
{
    public class InvalidDbOperationException : Exception
    {
        public InvalidDbOperationException()
        {
        }

        public InvalidDbOperationException(string message) : base(message)
        {
        }
    }
}