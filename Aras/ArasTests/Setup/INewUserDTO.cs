using ArasTests.Common.Aras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Setup {
    internal interface INewUserDTO {

        NewUserDTO GetNewUserDTO(string label);
    }
}
