using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataManager.Models;
using DataManager.Processors;
using GNNNeatLibrary.Controllers.Innovations;

namespace GNNNeatLibrary.Mutations
{
    /// <summary>
    /// Adds node by selecting random connection and
    /// inserting new node between two connected nodes
    /// thus modifying selected connection and creating
    /// one new connections and one node.
    /// </summary>
    public class AddNodeMutation: IMutation
    {
        private readonly INodeProcessor _nodeProcessor;
        private readonly IInnovationController _innovationController;
        private readonly IConnectionProcessor _connectionProcessor;

        public int GetWeight { get; set; } = 10;
        public int BatchId { get; set; } = 1;

        public AddNodeMutation(INodeProcessor nodeProcessor, IInnovationController innovationController, IConnectionProcessor connectionProcessor)
        {
            _nodeProcessor = nodeProcessor;
            _innovationController = innovationController;
            _connectionProcessor = connectionProcessor;
        }

        public void ApplyMutation(ref NetModel net)
        {
            // There is no connection thus no point of trying to insert node,
            // because it can only be inserted between two nodes AKA in connection
            if(net.Connections.Count == 0)
                return;

            // Finds connection - random
            var rnd = new Random();
            var connection = net.Connections[rnd.Next(0, net.Connections.Count)];

            // Gets innovation id for new node
            var innovation = _innovationController.FetchWithInsertOnFail(new InnovationModel
            {
                BatchId = net.BatchId,
                NodeFromId = connection.FromId,
                NodeToId = connection.ToId,
                Type = InnovationType.Node
            });

            // Makes two new connections, thus inserting new node between two others
            var connFromToNew = new ConnectionModel
            {
                FromId = connection.FromId,
                ToId = innovation.Id,
                NetworkId = net.Id,
                InnovationId = _innovationController.FetchWithInsertOnFail(new InnovationModel
                {
                    BatchId = net.BatchId,
                    NodeFromId = connection.FromId,
                    NodeToId = innovation.Id,
                    Type = InnovationType.Connection
                }).Id
            };
            _connectionProcessor.Save(ref connFromToNew);
            net.Connections.Add(connFromToNew);

            var connNewToTo = new ConnectionModel
            {
                FromId = innovation.Id,
                ToId = connection.ToId,
                NetworkId = net.Id,
                InnovationId = _innovationController.FetchWithInsertOnFail(new InnovationModel
                {
                    BatchId = net.BatchId,
                    NodeFromId = innovation.Id,
                    NodeToId = connection.ToId,
                    Type = InnovationType.Connection
                }).Id
            };
            _connectionProcessor.Save(ref connNewToTo);
            net.Connections.Add(connNewToTo);

            // Disables old connection
            connection.Enabled = false;
            _connectionProcessor.Update(connection);

            // Create new node only if it doesn't exist
            if (net.Nodes.Any(n => n.InnovationId == innovation.Id)) 
                return;


            var nodeModel = new NodeModel
            {
                InnovationId = innovation.Id,
                NetworkId = net.Id
            };

            _nodeProcessor.Save(ref nodeModel);
            net.Nodes.Add(nodeModel);
        }
    }
}
