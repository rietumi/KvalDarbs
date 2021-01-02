using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LogicCore.Util
{
    public class StringLengthLocalizedAttribute : StringLengthAttribute
    {
        public StringLengthLocalizedAttribute(int max)
            : base(max)
        {
            ErrorMessage = ErrorText.StringLength;
        }
    }
}
