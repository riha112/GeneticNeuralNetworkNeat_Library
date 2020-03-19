using DataManager.Models;
using GNNNeatLibrary.Utilities;

namespace GNNNeatLibrary.Controllers.Net
{
    public class MutationController : IMutationController
    {
        private readonly IMutationLibrary _mutationLibrary;

        public MutationController(IMutationLibrary mutationLibrary) =>
            _mutationLibrary = mutationLibrary;

        /// <summary>
        /// Applies random mutation to targeted network
        /// </summary>
        /// <param name="target">Network to mutate</param>
        public void Mutate(ref NetModel target)
        {
            var mutation = _mutationLibrary.GetRandomMutationBasedOnWeight();
            mutation?.ApplyMutation(ref target);
            // TODO: Test if mutation didn't break the network
        }

        public void MutateWithBatch(ref NetModel target, int batchId)
        {
            var mutation = _mutationLibrary.GetRandomMutationBasedOnWeightAndBatch(batchId);
            mutation?.ApplyMutation(ref target);
            // TODO: Test if mutation didn't break the network
            // if(broken(target)) then delete(target) and target = _netController.New(batchid)
        }

        public void MutateWithSpecific(ref NetModel target, int id)
        {
            var mutation = _mutationLibrary.GetMutation(id);
            mutation?.ApplyMutation(ref target);
        }
    }
}
