using DataManager.Models;

namespace GNNNeatLibrary.Controllers
{
    public interface INetNeatController
    {
        void FeedForward(ref NetModel target, double[] input);

        /// <summary>
        /// Finds networks innovation ranges min max
        /// </summary>
        /// <param name="net">Searchable network</param>
        /// <returns>tuple of minimum innovations id and maximum innovations id</returns>
        (int min, int max) GetInnovationRange(NetModel net);
    }
}