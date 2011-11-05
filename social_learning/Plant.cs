﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace social_learning
{
    public class Plant
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PlantSpecies Species { get; set; }

        private HashSet<IAgent> whosEatenMe = new HashSet<IAgent>();

        public Plant(PlantSpecies species)
        {
            Species = species;
        }

        public bool AvailableForEating(IAgent agent)
        {
            return !whosEatenMe.Contains(agent);
        }

        public void EatenBy(IAgent agent)
        {
            whosEatenMe.Add(agent);
        }
    }
}
