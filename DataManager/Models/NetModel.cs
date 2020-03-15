using System;
using System.Collections.Generic;
using System.Text;

namespace DataManager.Models
{
    public class NetModel
    {
        public int Id { get; set; }
        public double FitnessScore { get; set; }
        public int BirthGeneration { get; set; }

        public List<NodeModel> Nodes { get; set; } = new List<NodeModel>();
        public List<ConnectionModel> Connections { get; set; } = new List<ConnectionModel>();

    }
}
