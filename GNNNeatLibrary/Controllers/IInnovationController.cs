using DataManager.Models;

namespace GNNNeatLibrary.Controllers
{
    public interface IInnovationController
    {
        /// <summary>
        /// If innovation exists in database then it's returned, otherwise
        /// new innovation is created and returned. 
        /// </summary>
        /// <param name="innovation">Fetch-able innovation</param>
        /// <returns>Linked innovation</returns>
        InnovationModel FetchWithInsertOnFail(InnovationModel innovation);
    }
}