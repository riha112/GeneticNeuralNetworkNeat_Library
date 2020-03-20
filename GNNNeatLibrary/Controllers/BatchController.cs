using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataManager.Models;
using DataManager.Processors;
using GNNNeatLibrary.Controllers.Net;
using GNNNeatLibrary.Utilities;

namespace GNNNeatLibrary.Controllers
{
    public class BatchController : IBatchController
    {
        private readonly IBatchProcessor _batchProcessor;
        private readonly IInnovationProcessor _innovationProcessor;
        private readonly INetController _netController;

        public BatchController(IBatchProcessor batchProcessor, IInnovationProcessor innovationProcessor, INetController netController) =>
            (_batchProcessor, _innovationProcessor, _netController) = (batchProcessor, innovationProcessor, netController);

        public BatchModel New(string name, string description = "")
        {
            // Creates new batch model
            var model = new BatchModel
            {
                Name = name,
                Description = description
            };
             _batchProcessor.Save(ref model);

            InitializeInputOutputNodePreset(ref model);
            InitializeGenOneNetworks(ref model);

            if (Config.AddRandomConnectionMutationsOnStartMax != 0)
                return _batchProcessor.Load(model.Id);

            return model;
        }

        private void InitializeInputOutputNodePreset(ref BatchModel batchModel)
        {
            for (var i = 0; i < Config.InputLayerSize + Config.OutputLayerSize; i++)
            {
                var innovation = new InnovationModel
                {
                    BatchId = batchModel.Id,
                    NodeFromId = 0,
                    NodeToId = 0,
                    Type = InnovationType.PresetNode
                };
                _innovationProcessor.Save(ref innovation);
                batchModel.Innovations.Add(innovation);
            }
        }

        public void IncreaseGeneration(BatchModel batchModel)
        {
            batchModel.Generation++;
            Save(batchModel);
        }

        public void UpdateBestPerformingNetwork(BatchModel batchModel)
        {
            batchModel.BestPerformingNetwork =
                batchModel.Networks.OrderByDescending(n => n.FitnessScore).First().Id;
            Save(batchModel);
        }

        private void InitializeGenOneNetworks(ref BatchModel batchModel)
        {
            for (var i = 0; i < Config.NetworkCountPerPopulation; i++)
            {
                var netModel = _netController.New(batchModel);
                batchModel.Networks.Add(netModel);
            }
        }

        public BatchModel Load(int id)
        {
           return _batchProcessor.Load(id);
        }

        public void Kill(BatchModel target)
        {
            _ = target ?? throw new NullReferenceException();
            _batchProcessor.Delete(target.Id);
        }

        public void Save(BatchModel target)
        {
            _ = target ?? throw new NullReferenceException();
            _batchProcessor.Update(target);
        }

    }
}
