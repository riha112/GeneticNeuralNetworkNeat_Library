using System;
using System.Collections.Generic;
using System.Text;

namespace DataManager.Models
{
    public enum InnovationType: short
    {
        Connection = 0,
        Node = 1
    }

    public class InnovationModel
    {
        public int Id { get; set; }
        public int NodeFromId { get; set; }
        public int NodeToId { get; set; }
        public int BatchId { get; set; }

        public InnovationType Type { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
