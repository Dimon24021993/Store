﻿using System;

namespace Store.BLL.Exceptions
{
    public class InvalidModelException : Exception
    {
        public InvalidModelException()
        {
        }

        public InvalidModelException(string message) : base(message)
        {
        }
    }
}