using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCore
{
    public class TrainingTask
    {
        public Training Training { get; set; }
        public int TrainingId { get; set; }
        public Task Task { get; set; }
        public int TaskId { get; set; }
    }
}
