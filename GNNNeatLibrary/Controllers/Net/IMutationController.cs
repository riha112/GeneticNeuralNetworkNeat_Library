using DataManager.Models;

namespace GNNNeatLibrary.Controllers.Net
{
    public interface IMutationController
    {
        /// <summary>
        /// Applies random mutation to targeted network
        /// </summary>
        /// <param name="target">Network to mutate</param>
        void Mutate(ref NetModel target);

        void MutateWithBatch(ref NetModel target, int batchId);
        void MutateWithSpecific(ref NetModel target, int id);
    }
}