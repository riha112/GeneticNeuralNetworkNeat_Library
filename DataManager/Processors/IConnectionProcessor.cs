using System.Collections.Generic;
using DataManager.Models;

namespace DataManager.Processors
{
    public interface IConnectionProcessor
    {
        ConnectionModel Load(int id);
        List<ConnectionModel> LoadLinked(int networkId);
        void Save(ref ConnectionModel connectionModel);
        void Delete(int connectionId);
        void Update(ConnectionModel connectionModel);
    }
}