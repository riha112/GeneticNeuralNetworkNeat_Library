using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Processors;

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
        //private INodeProcessor _nodeProcessor;
        public int GetWeight { get; set; } = 10;

        //public AddNodeMutation(INodeProcessor nodeProcessor) => _nodeProcessor = nodeProcessor;

        public void ApplyMutation(ref NetModel net)
        {
            // There is no connection thus no point of trying to insert node,
            // because it can only be inserted between two nodes AKA in connection
            if(net.Connections.Count == 0)
                return;
            
            // Finds connection - random

            throw new NotImplementedException();
        }
    }
}
