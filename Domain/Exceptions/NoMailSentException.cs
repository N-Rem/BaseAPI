using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NoMailSentException: Exception
    {
        public NoMailSentException()
      : base()
        {
        }

        public NoMailSentException(string message)
            : base(message)
        {
        }

        public NoMailSentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NoMailSentException(string name, object key)
            : base($"Could not send email.")
        {
        }
    }
}
