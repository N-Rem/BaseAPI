using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class RestoreCodeValidationException : Exception
    {
        public RestoreCodeValidationException()
        : base()
        {
        }

        public RestoreCodeValidationException(string message)
            : base(message)
        {
        }

        public RestoreCodeValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RestoreCodeValidationException(string name, object key)
            : base($"Invalid recovery code.")
        {
        }
    }
}
