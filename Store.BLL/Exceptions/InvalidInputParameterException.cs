using System;

namespace Store.BLL.Exceptions
{
    public class InvalidInputParameterException : Exception
    {
        public InvalidInputParameterException()
        {
        }

        public InvalidInputParameterException(string message) : base(message)
        {
        }
    }
}