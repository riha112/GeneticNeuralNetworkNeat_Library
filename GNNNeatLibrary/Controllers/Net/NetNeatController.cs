using System;
using System.Collections.Generic;
using System.Linq;
using DataManager.Models;
using GNNNeatLibrary.Utilities;

namespace GNNNeatLibrary.Controllers.Net
{
    public class NetNeatController : INetNeatController
    {
        public double[] GetOutputValues(NetModel target)
        {
            var output = new double[Config.OutputLayerSize];
            var offset = Config.InputLayerSize;
            for (var i = 0; i < Config.OutputLayerSize; i++)
                output[i] = target.Nodes[i + offset].Value;

            return output;
        }

        public void FeedForward(NetModel target, double[] input)
        {
            // In network model input layer and output layers are stored in sequence
            // Example for 3in and 1out: List<Nodes>[in, in, in, out, hid, hid, hid ..]

            #region Prepairing for feed forward
            
            var (inputCount, outputCount) = (Config.InputLayerSize, Config.OutputLayerSize);

            if (input.Length != inputCount)
                throw new Exception($"{nameof(input)} should be {inputCount} long.");

            for (var i = 0; i < inputCount; i++)
                target.Nodes[i].Value = input[i];

            #endregion


            #region FeedForward implementation

            // Because of the algorithm the network can be non-linear, meaning that
            // it's possible to create connection between nodes in layers that are not
            // next to one another, for example input layer can connect to output layer
            // even thou there are hidden layers in between.

            var currentLayersNodes = new HashSet<int>();

            // First run - input layer
            for (var i = 0; i < inputCount; i++)
                currentLayersNodes.Add(target.Nodes[i].InnovationId);

            var runCount = 0;
            do
            {
                runCount++;
                currentLayersNodes = UpdateLayerAndGetNext(ref target, currentLayersNodes, runCount == 1);
            } while (currentLayersNodes.Count != 0 && runCount < target.Connections.Count);
            #endregion
        }

        private HashSet<int> UpdateLayerAndGetNext(ref NetModel target, HashSet<int> currentLayer, bool isInputLayer = false)
        {
            var nextLayer = new HashSet<int>();
            var updatedValues = new Dictionary<int, double>();

            foreach (var connection in target.Connections.Where(connection => connection.Enabled))
            {
                // Builds next layer
                if (currentLayer.Contains(connection.FromId))
                    nextLayer.Add(connection.ToId);

                // If it's a input layer then we have no need to update node values
                if(isInputLayer)
                    continue;

                // If we have reached this node it means that all previous nodes have been set
                // thus we can find all connections that are marked towards current layers node
                // to calculate its new value
                if (currentLayer.Contains(connection.ToId))
                {
                    if(!updatedValues.ContainsKey(connection.ToId))
                        updatedValues.Add(connection.ToId, 0);

                    // Basics of NN the value of node is sum of previous layer nodes * weight of
                    // connection that connects them
                    updatedValues[connection.ToId] += connection.Weight * FindNode(target, connection.FromId).Value;
                }
            }

            // After we have calculated the sum of new value we can apply it through our activation function - Tanh.
            foreach (var (innovationId, value) in updatedValues)
                FindNode(target, innovationId).Value = Math.Tanh(value);

            return nextLayer;
        }

        private static NodeModel FindNode(NetModel target, int innovationId)
        {
            return target.Nodes.FirstOrDefault(n => n.InnovationId == innovationId);
        }

        /// <summary>
        /// Finds networks innovation ranges min max
        /// </summary>
        /// <param name="net">Searchable network</param>
        /// <returns>tuple of minimum innovations id and maximum innovations id</returns>
        public (int min, int max) GetInnovationRange(NetModel net)
        {
            (int min, int max) minMxTuple = (0, 0);

            // Connections and innovations are sorted on gnn population
            // and selection from DB
            minMxTuple.min = net.Connections[0].InnovationId;
            minMxTuple.max = net.Connections.Last().InnovationId;

            //// Connections / Innovations are not sorted, thus
            //// we need to search through them in O(n) complexity
            //foreach (var connection in net.Connections)
            //{
            //    if (connection.InnovationId < minMxTuple.min)
            //        minMxTuple.min = connection.InnovationId;
            //    else if (connection.InnovationId > minMxTuple.max)
            //        minMxTuple.max = connection.InnovationId;
            //}

            return minMxTuple;
        }

    }
}
