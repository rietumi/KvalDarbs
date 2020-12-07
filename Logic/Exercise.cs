using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Exercise : IEntity
    {
        public List<int> Examples { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public int Id
        {
            get => default;
            set
            {
            }
        }
    }
}