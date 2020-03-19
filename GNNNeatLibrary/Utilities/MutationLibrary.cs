using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GNNNeatLibrary.Mutations;
using System.Linq;
using DataManager.Processors;
using GNNNeatLibrary.Controllers.Innovations;

namespace GNNNeatLibrary.Utilities
{
    public class MutationLibrary : IMutationLibrary
    {
        private static readonly List<IMutation> Mutations = new List<IMutation>();
        private static readonly Dictionary<int, List<IMutation>> BatchedMutations = new Dictionary<int, List<IMutation>>();
        private static int _totalWeight = 0;

        private readonly IConnectionProcessor _connectionProcessor;
        private readonly INodeProcessor _nodeProcessor;
        private readonly IInnovationController _innovationController;

        public MutationLibrary(IConnectionProcessor connectionProcessor, INodeProcessor nodeProcessor, IInnovationController innovationController)
        {
            _connectionProcessor = connectionProcessor;
            _nodeProcessor = nodeProcessor;
            _innovationController = innovationController;

            if (Mutations.Count != 0) return;

            BuildLibrary();

            //_mutations = _mutations.OrderByDescending(m => m.GetWeight).ToList();
            _totalWeight = (int)Mutations.Sum(m => m.GetWeight);
        }

        private void BuildLibrary()
        {
            Mutations.Add(new AddNodeMutation(_nodeProcessor, _innovationController, _connectionProcessor));
            Mutations.Add(new AddConnectionMutation(_connectionProcessor, _innovationController));
            Mutations.Add(new WeightChangeMutation(_connectionProcessor));
            Mutations.Add(new WeightShiftMutation(_connectionProcessor));

            // Builds dictionary : key: batch_id, value: List of mutations
            foreach (var mutation in Mutations)
            {
                if (!BatchedMutations.ContainsKey(mutation.BatchId))
                    BatchedMutations.Add(mutation.BatchId, new List<IMutation>());
                BatchedMutations[mutation.BatchId].Add(mutation);
            }
        }

        public IMutation GetRandomMutationBasedOnWeight()
        {
            var rnd = new Random();
            var selected = rnd.Next(0, _totalWeight);
            var currentSum = 0;

            for (var i = 0; i < Mutations.Count - 1; i++)
            {
                currentSum += Mutations[i].GetWeight;
                if (selected <= currentSum)
                    return Mutations[i];
            }

            return Mutations.Last();
        }

        public IMutation GetRandomMutationBasedOnWeightAndBatch(int batchId)
        {
            var rnd = new Random();
            var selected = rnd.Next(0, 100);
            var totalBatchSum = BatchedMutations[batchId].Sum(t => t.GetWeight);

            if (selected > totalBatchSum)
                return null;

            var currentSum = 0;
            for (var i = 0; i < BatchedMutations[batchId].Count - 1; i++)
            {
                currentSum += BatchedMutations[batchId][i].GetWeight;
                if (selected <= currentSum)
                    return BatchedMutations[batchId][i];
            }

            return BatchedMutations[batchId].Last();
        }

        public IMutation GetRandomMutation()
        {
            var rnd = new Random();
            return Mutations[rnd.Next(0, Mutations.Count)];
        }

        public IMutation GetMutation(int id)
        {
            if (id < 0 || id > Mutations.Count)
                return null;

            return Mutations[id];
        }
    }
}
