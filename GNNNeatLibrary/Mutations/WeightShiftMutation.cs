using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Processors;

namespace GNNNeatLibrary.Mutations
{
    /// <summary>
    /// Changes random connections weight, so that if connections weight is negative it stays negative and
    /// if it's positive it stays positive
    /// </summary>
    public class WeightShiftMutation : IMutation
    {
        private readonly IConnectionProcessor _connectionProcessor;

        public int GetWeight { get; set; } = 45;
        public int BatchId { get; set; } = 3;

        public WeightShiftMutation(IConnectionProcessor connectionProcessor)
        {
            _connectionProcessor = connectionProcessor;
        }

        public void ApplyMutation(ref NetModel net)
        {
            if(net.Connections.Count == 0)
                return;
            
            // Shifts weight
            var rnd = new Random();
            var connectionId = rnd.Next(0, net.Connections.Count);
            net.Connections[connectionId].Weight *= rnd.NextDouble();

            // Saves change
            _connectionProcessor.Update(net.Connections[connectionId]);
        }
    }
}

