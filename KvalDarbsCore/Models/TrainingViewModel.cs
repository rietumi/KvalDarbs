using System;
using System.Collections.Generic;
using LogicCore;

namespace KvalDarbsCore.Models
{
    public class TrainingViewModel
    {
        public string User { get; set; }
        public List<TaskViewModel> Tasks { get; set; }
    }
}
