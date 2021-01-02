using System.ComponentModel.DataAnnotations;

namespace LogicCore
{
    public enum CompetitionType
    {
        [Display(Name = "Maratons")]
        Marathon = 1,

        [Display(Name = "Pusmaratons")]
        HalfMarathon = 2,

        [Display(Name = "10km")]
        TenKilometers = 3,

        [Display(Name = "5km")]
        FiveKilometers = 4,

        [Display(Name = "Sprints")]
        Sprint = 5
    }
}