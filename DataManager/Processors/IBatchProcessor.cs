using DataManager.Models;

namespace DataManager.Processors
{
    public interface IBatchProcessor
    {
        BatchModel Load(int id);
        void Save(ref BatchModel batchModel);
        void Delete(int batchId);
        void Update(BatchModel batchModel);
    }
}