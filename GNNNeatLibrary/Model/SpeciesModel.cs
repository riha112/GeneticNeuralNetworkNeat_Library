using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;

namespace GNNNeatLibrary.Model
{
    public class SpeciesModel
    {
        public NetModel Head { get; set; }
        public List<NetModel> Members { get; set; } = new List<NetModel>();
        public double AverageFitnessScore { get; set; }
    }
}
