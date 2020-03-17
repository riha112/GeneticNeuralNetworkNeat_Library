using System.Collections.Generic;
using DataManager.Models;

namespace DataManager.Processors
{
    public interface INetworkProcessor
    {
        NetModel Load(int id);
        List<NetModel> LoadLinked(int batchId);
        void Save(ref NetModel netModel);
        void Delete(int netId);
        void Update(NetModel netModel);
    }
}