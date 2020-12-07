using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public interface IValidation
    {
        bool Validate(ref List<Error> state);
    }
}