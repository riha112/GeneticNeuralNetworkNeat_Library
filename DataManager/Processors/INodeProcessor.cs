using System.Collections.Generic;
using DataManager.Models;

namespace DataManager.Processors
{
    public interface INodeProcessor
    {
        NodeModel Load(int id);
        List<NodeModel> LoadLinked(int networkId);
        void Save(ref NodeModel nodeModel);
        void Delete(int nodeId);
        void Update(NodeModel nodeModel);
    }
}