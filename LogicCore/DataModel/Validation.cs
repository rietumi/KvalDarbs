using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicCore
{
    public interface IValidation
    {
        bool Validate(ref List<Error> state);
    }
}