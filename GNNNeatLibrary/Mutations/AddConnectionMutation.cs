using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Processors;
using GNNNeatLibrary.Controllers.Innovations;
using GNNNeatLibrary.Utilities;

namespace GNNNeatLibrary.Mutations
{
    public class AddConnectionMutation : IMutation
    {
        private readonly IConnectionProcessor _connectionProcessor;
        private readonly IInnovationController _innovationController;
        public int GetWeight { get; set; } = 10;
        public int BatchId { get; set; } = 2;

        public AddConnectionMutation(IConnectionProcessor connectionProcessor, IInnovationController innovationController)
        {
            _connectionProcessor = connectionProcessor;
            _innovationController = innovationController;
        }

        public void ApplyMutation(ref NetModel net)
        {
            var (fromId, toId) = GetRandomNodeIds(net);

            if (HasConnection(net, fromId, toId) is { } id)
            {
                ToggleConnection(ref net, id);
                return;
            }

            // Finds innovation number based on connection locations: from, to
            var innovation = _innovationController.FetchWithInsertOnFail(new InnovationModel
            {
                BatchId = net.BatchId,
                NodeFromId = fromId,
                NodeToId = toId,
                Type = InnovationType.Connection
            });

            // Creates new connections
            var rnd = new Random();
            var connection = new ConnectionModel
            {
                FromId = fromId,
                ToId = toId,
                InnovationId = innovation.Id,
                NetworkId = net.Id,
                Weight = rnd.NextDouble() * 2 - 1 // Range of -1 to 1
            };

            // Saves and Adds new connection to network
            _connectionProcessor.Save(ref connection);
            net.Connections.Add(connection);
        }

        private static int? HasConnection(NetModel net, int fromId, int toId)
        {
            // If connection exists in one way that means that you can not add the connection other way
            // because it will create an infinity loop.
            for (var i = 0; i < net.Connections.Count; i++)
            {
                var c = net.Connections[i];
                if ((c.FromId == fromId && c.ToId == toId) || (c.FromId == toId && c.ToId == fromId))
                    return i;
            }
            return null;
        }

        /// <summary>
        /// Finds two potential nodes for connections, so that - node from can not be output layers node
        /// and node to can not be input layers node
        /// </summary>
        /// <param name="net">Where to search nodes</param>
        /// <returns>Tuple containing from and to nodes innovation ids</returns>
        private static (int fromId, int toId) GetRandomNodeIds(NetModel net)
        {
            var rnd = new Random();

            // Output layer can't connect to other other nodes, thus:
            var from = rnd.Next(0, net.Nodes.Count - Config.OutputLayerSize );

            // Input layer can't be targeted by other nodes, thus:
            var to = rnd.Next(Config.InputLayerSize, net.Nodes.Count);

            return (net.Nodes[from].InnovationId, net.Nodes[to].InnovationId);
        }

        /// <summary>
        /// Changes enabled/disabled state of specific connection
        /// </summary>
        /// <param name="net">Network that contains connection</param>
        /// <param name="connId">Connections id int list (not db id and not innovation id)</param>
        private void ToggleConnection(ref NetModel net, int connId)
        {
            net.Connections[connId].Enabled = !net.Connections[connId].Enabled;
            _connectionProcessor.Update(net.Connections[connId]); 
        }
    }
}

