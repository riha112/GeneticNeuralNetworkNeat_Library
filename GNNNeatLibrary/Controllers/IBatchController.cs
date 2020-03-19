using DataManager.Models;

namespace GNNNeatLibrary.Controllers
{
    public interface IBatchController
    {
        BatchModel New(string name, string description = "");
        BatchModel Load(int id);
        void Kill(ref BatchModel target);
        void Save(ref BatchModel target);
    }
}