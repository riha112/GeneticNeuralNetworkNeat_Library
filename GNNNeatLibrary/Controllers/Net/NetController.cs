using System;
using DataManager.Models;
using DataManager.Processors;
using GNNNeatLibrary.Utilities;

namespace GNNNeatLibrary.Controllers.Net
{
    public class NetController : INetController
    {
        private readonly INetworkProcessor _networkProcessor;
        private readonly INodeProcessor _nodeProcessor;
        private readonly IConnectionProcessor _connectionProcessor;
        private readonly IMutationController _mutationController;

        public NetController(INetworkProcessor networkProcessor, INodeProcessor nodeProcessor, IConnectionProcessor connectionProcessor, IMutationController mutationController) =>
            (_networkProcessor, _nodeProcessor, _connectionProcessor, _mutationController) = (networkProcessor, nodeProcessor, connectionProcessor, mutationController);

        public NetModel New(BatchModel parenBatchModel)
        {
            var model = new NetModel
            {
                BatchId = parenBatchModel.Id,
                BirthGeneration = parenBatchModel.Generation
            };
            _networkProcessor.Save(ref model);

            // Adds input and output layers nodes
            for (var i = 0; i < Config.InputLayerSize + Config.OutputLayerSize; i++)
            {
                var nodeModel = new NodeModel
                {
                    NetworkId = model.Id,
                    InnovationId = parenBatchModel.Innovations[i].Id,
                };
                _nodeProcessor.Save(ref nodeModel);
                model.Nodes.Add(nodeModel);
            }

            // Adds random connections
            var rnd = new Random();
            var addMutationCount = rnd.Next(Config.AddRandomConnectionMutationsOnStartMin,
                Config.AddRandomConnectionMutationsOnStartMax);
            for(var i = 0; i < addMutationCount; i++)
                _mutationController.MutateWithSpecific(ref model, 1);

            return model;
        }

        public void AddConnection(ref NetModel target, ConnectionModel connection)
        {
            var model = new ConnectionModel
            {
                Enabled = connection.Enabled,
                FromId = connection.FromId,
                ToId = connection.ToId,
                InnovationId = connection.InnovationId,
                Weight = connection.Weight,
                NetworkId = target.Id
            };

            _connectionProcessor.Save(ref model);
            target.Connections.Add(model);
        }

        public void AddNode(ref NetModel target, NodeModel node)
        {
            var model = new NodeModel
            {
                InnovationId = node.InnovationId,
                NetworkId = target.Id
            };

            _nodeProcessor.Save(ref model);
            target.Nodes.Add(model);
        }

        public void Kill(NetModel target)
        {
            _ = target ?? throw new NullReferenceException();
            _networkProcessor.Delete(target.Id);
        }

        public void Save(NetModel target)
        {
            _ = target ?? throw new NullReferenceException();
            _networkProcessor.Update(target);
        }

    }
}
