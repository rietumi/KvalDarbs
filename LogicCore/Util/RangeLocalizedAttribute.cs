using System.ComponentModel.DataAnnotations;

namespace LogicCore.Util
{
    public class RangeLocalizedAttribute : RangeAttribute
    {
        public RangeLocalizedAttribute(int min, int max) : base(min, max)
        {
            ErrorMessage = ErrorText.Range;
        }
    }
}
