using System.ComponentModel.DataAnnotations;

namespace LogicCore.Util
{
    public class RequiredLocalizedAttribute : RequiredAttribute
    {
        public RequiredLocalizedAttribute() : base()
        {
            ErrorMessage = ErrorText.Required;
        }
    }
}
