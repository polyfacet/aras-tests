using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Arranging
{
    internal class ArrangeException : Exception
    {
        public ArrangeException(string message, Exception ex) : base(message, ex) {

        }
    }
}
