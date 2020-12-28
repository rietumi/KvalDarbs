using System.Collections.Generic;

namespace LogicCore
{
    public interface IValidation
    {
        bool Validate(ref Dictionary<string, string> state);
    }
}