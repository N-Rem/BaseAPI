using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotValidFormatExeption : Exception
    {
        public NotValidFormatExeption()
        : base()
        {
        }

        public NotValidFormatExeption(string message)
            : base(message)
        {
        }

        public NotValidFormatExeption(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotValidFormatExeption(string name, object key)
            : base($"Entity {name} ({key}) was not Valid.")
        {
        }
    }
}
