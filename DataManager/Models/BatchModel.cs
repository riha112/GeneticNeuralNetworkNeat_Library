using System;
using System.Collections.Generic;
using System.Text;

namespace DataManager.Models
{
    /// <summary>
    /// Class that represents databases table: Batch
    /// Used as holder of training set instances.
    /// </summary>
    public class BatchModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Generation { get; set; }

        public int BestPerformingNetwork { get; set; }

        public DateTime CreationDate { get; set; }

        public List<NetModel> Networks { get; set; } = new List<NetModel>();
        public List<InnovationModel> Innovations { get; set; } = new List<InnovationModel>();
    }
}
