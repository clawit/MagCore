using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model.Exceptions
{
    public class WrongFormatException : Exception
    {
        public WrongFormatException(Exception innerException) : base("Wrong format data", innerException)
        {

        }
    }
}
