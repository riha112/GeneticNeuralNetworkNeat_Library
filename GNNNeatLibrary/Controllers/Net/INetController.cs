using DataManager.Models;

namespace GNNNeatLibrary.Controllers.Net
{
    public interface INetController
    {
        NetModel New(BatchModel parenBatchModel, bool addRandom = true);
        void AddConnection(ref NetModel target, ConnectionModel connection);
        void AddNode(ref NetModel target, NodeModel connection);
        void Kill(NetModel target);
        void Save(NetModel target);
    }
}