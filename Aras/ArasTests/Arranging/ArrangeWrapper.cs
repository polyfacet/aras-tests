using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArasTests.Basics.ArasConnectionTests;

namespace ArasTests.Arranging
{
    internal class ArrangeWrapper
    {
        private Action _action;
        public ArrangeWrapper(Action arrangementAction) {
            _action = arrangementAction;
        }

        public void Run() {
            try {
                _action();
            }
            catch (Exception ex) {
                throw new ArrangeException("Arrange exception: ", ex);
            }
        }

        public static void Run(Action action) {
            try {
                action();
            }
            catch (Exception ex) {
                throw new ArrangeException("Arrange exception: ", ex);
            }
        }
    }
}
