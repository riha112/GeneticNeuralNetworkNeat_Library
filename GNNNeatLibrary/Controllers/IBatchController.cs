using DataManager.Models;

namespace GNNNeatLibrary.Controllers
{
    public interface IBatchController
    {
        BatchModel New(string name, string description = "");
        BatchModel Load(int id);
        void UpdateBestPerformingNetwork(BatchModel batchModel);
        void IncreaseGeneration(BatchModel batchModel);
        void Kill(BatchModel target);
        void Save(BatchModel target);
    }
}