using System;
using System.Collections.Generic;
using DataManager.Models;
using GNNNeatLibrary.Controllers.Innovations;

namespace GNNNeatLibrary.Controllers.Net
{
    public class BreedingController : IBreedingController
    {
        private readonly INetNeatController _netNeatController;
        //private IInnovationController _innovationController;
        private readonly INetController _netController;

        public BreedingController(
            INetNeatController netNeatController,
            //IInnovationController innovationController,
            INetController netController)
        {
            _netNeatController = netNeatController;
           // _innovationController = innovationController;
            _netController = netController;
        }

        public NetModel Breed(BatchModel family, NetModel mother, NetModel father)
        {
            var rnd = new Random();

            // Mother should always be fitter then father
            if (mother.FitnessScore < father.FitnessScore)
                return Breed(family,father, mother);

            // Quick rules for how offspring is generated
            // If both parents have the same innovation then take a random one
            // If fittest parent have innovation then take it.
            // Otherwise skip.

            // As we only care for fittest parents innovation range
            // we can ignore the range of weaker one.
            // - In first check we have ensured that mother is always the fittest one.
            var range = _netNeatController.GetInnovationRange(mother);
            var parents = new NetModel[2] { mother, father };

            // Each connection has an innovation id, thus we can
            // Create array where each connection is stored in cell
            // based on its innovations id.
            // We store connections id in table.
            var table = new int[2, range.max - range.min + 1];
            for(var i = 0; i < 2; i++)
                for (var x = 0; x < table.GetLength(1); x++)
                    table[i, x] = -1;

            for (var p = 0; p < 2; p++)
            {
                for (var c = 0; c < parents[p].Connections.Count; c++)
                {
                    var column = parents[p].Connections[c].InnovationId - range.min;
                    // Father can go outside range.
                    if (column < 0 || column >= table.GetLength(1))
                        continue;

                    table[p, column] = c;
                }
            }

            var child = _netController.New(family, false);

            // Populates child with connections
            for (var c = 0; c < range.max - range.min + 1; c++)
            {
                // Disjoint on mother, thus we don't care about the connection 
                if (table[0, c] == -1)
                    continue;

                // If both are empty skip
                if (table[0, c] == -1 && table[1, c] == -1) 
                    continue;

                if (table[0, c] != -1 && table[1, c] != -1)
                {
                    // Adds random connection to child
                    var parentId = rnd.Next(0, 2);
                    _netController.AddConnection(ref child, parents[parentId].Connections[table[parentId, c]]);
                    continue;
                }

                // If disjoint on father, then add mother connection
                _netController.AddConnection(ref child, mother.Connections[table[0,c]]);
            }

            // Populates child with nodes
            var initializedNodeIds = new HashSet<int>();
            foreach (var node in child.Nodes)
                initializedNodeIds.Add(node.InnovationId);

            foreach (var connection in child.Connections)
            {
                // If child doesn't contain node then add it to network and initializedNode set
                var currentIds = new[] {connection.ToId, connection.FromId};
                foreach (var currentId in currentIds)
                {
                    if (initializedNodeIds.Contains(currentId)) 
                        continue;

                    _netController.AddNode(ref child, new NodeModel
                    {
                        InnovationId = currentId
                    });
                    initializedNodeIds.Add(currentId);
                }
            }

            return child;
        }
    }
}
