﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_IGA
{
    /// <summary>
    /// 种群
    /// </summary>
    internal class Population
    {
        public List<Individual> Individuals;
        public Population(Individual[] individuals)
        {
            Individuals = individuals.ToList();
        }
    }
}
