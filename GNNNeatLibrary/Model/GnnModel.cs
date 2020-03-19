using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;

namespace GNNNeatLibrary.Model
{
    public class GnnModel
    {
        public List<SpeciesModel> Species { get; set; } = new List<SpeciesModel>();
        public List<NetModel> UnsignedNetworks { get; set; } = new List<NetModel>();
        public BatchModel ActiveBatchModel { get; set; }
        public double FullFitnessScore { get; set; } = 0;
    }
}
