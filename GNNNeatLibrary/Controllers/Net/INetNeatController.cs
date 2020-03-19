using DataManager.Models;

namespace GNNNeatLibrary.Controllers.Net
{
    public interface INetNeatController
    {
        double[] GetOutputValues(NetModel target);

        void FeedForward(NetModel target, double[] input);

        /// <summary>
        /// Finds networks innovation ranges min max
        /// </summary>
        /// <param name="net">Searchable network</param>
        /// <returns>tuple of minimum innovations id and maximum innovations id</returns>
        (int min, int max) GetInnovationRange(NetModel net);
    }
}