using GNNNeatLibrary.Mutations;

namespace GNNNeatLibrary.Utilities
{
    public interface IMutationLibrary
    {
        IMutation GetMutation(int id);
        IMutation GetRandomMutation();
        IMutation GetRandomMutationBasedOnWeight();
        IMutation GetRandomMutationBasedOnWeightAndBatch(int batchId);

    }
}
