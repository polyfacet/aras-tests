using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests
{
    internal class InnovatorBase
    {
        protected readonly Innovator.Client.IOM.Innovator Inn;
        public InnovatorBase(Innovator.Client.IOM.Innovator inn) {
            Inn = inn;
        }
    }
}
