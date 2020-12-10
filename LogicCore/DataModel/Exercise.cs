using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicCore
{
    public class Exercise : IEntity
    {
        public List<Example> Examples { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }
    }
}