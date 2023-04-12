using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Mycronic
{
    internal class MycronicBase : InnovatorBase
    {
        public Parts.Part Part;

        public MycronicBase(Innovator.Client.IOM.Innovator inn) : base(inn) {
            Part = new Parts.Part(Inn);
        }
    }
}
