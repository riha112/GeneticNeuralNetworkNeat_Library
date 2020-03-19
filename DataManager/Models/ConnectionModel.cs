using System;
using System.Collections.Generic;
using System.Text;

namespace DataManager.Models
{
    public class ConnectionModel
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public double Weight { get; set; }
        public int NetworkId { get; set; }

        public int InnovationId { get; set; }
        public bool Enabled { get; set; }
    }
}
