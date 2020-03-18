using System.Collections.Generic;
using DataManager.Models;

namespace DataManager.Processors
{
    public interface IInnovationProcessor
    {
        InnovationModel Load(int id);
        List<InnovationModel> LoadLinked(int batchId);
        InnovationModel Find(InnovationType type, int from, int to, int batchId);
        void Save(ref InnovationModel innovationModel);
        void Delete(int innovationId);
        void Update(InnovationModel innovationModel);
    }
}