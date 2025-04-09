using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class RestoreCodeTimeException : Exception
    {
        public RestoreCodeTimeException()
        : base()
        {
        }

        public RestoreCodeTimeException(string message)
            : base(message)
        {
        }

        public RestoreCodeTimeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RestoreCodeTimeException(string name, object key)
            : base($"The code has expired.")
        {
        }
    }
}
