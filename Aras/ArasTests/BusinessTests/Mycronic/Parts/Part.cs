using Innovator.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Mycronic.Parts
{
    internal class Part : InnovatorBase
    {
        public readonly Create Create;

        public Part(Innovator.Client.IOM.Innovator inn) : base(inn) {
            Create = new Create(Inn);
        }

    }
}
