using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Processors;

namespace GNNNeatLibrary.Mutations
{
    /// <summary>
    /// Assigns completely new weight to random connection
    /// </summary>
    public class WeightChangeMutation : IMutation
    {

        private readonly IConnectionProcessor _connectionProcessor;

        public int GetWeight { get; set; } = 5;
        public int BatchId { get; set; } = 3;

        public WeightChangeMutation(IConnectionProcessor connectionProcessor)
        {
            _connectionProcessor = connectionProcessor;
        }

        public void ApplyMutation(ref NetModel net)
        {
            if (net.Connections.Count == 0)
                return;

            // Shifts weight
            var rnd = new Random();
            var connectionId = rnd.Next(0, net.Connections.Count);
            net.Connections[connectionId].Weight *= rnd.NextDouble() * 2 - 1; // Range -1 to 1

            // Saves change
            _connectionProcessor.Update(net.Connections[connectionId]);
        }
    }
}
