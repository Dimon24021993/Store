using System;

namespace Store.BLL.Exceptions
{
    public class ImpossibleDeleteException : Exception
    {
        public ImpossibleDeleteException()
        {
        }

        public ImpossibleDeleteException(string message) : base(message)
        {
        }
    }
}