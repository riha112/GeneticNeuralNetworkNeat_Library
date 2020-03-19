using GNNNeatLibrary.Model;

namespace GNNNeatLibrary.Controllers
{
    public interface IGnnController
    {
        GnnModel Load(int batchId);
        GnnModel New(string name, string description);
        /// <summary>
        /// From unsigned network list fetches all networks and splits them into
        /// families/species, witch is done by using Neat network similarity algorithm.
        /// </summary>
        /// <param name="gnnModel">Network to specify</param>
        void SpecifyNetworks(ref GnnModel gnnModel);

        void EvaluateNetworks(ref GnnModel gnnModel);
        void PopulateNextGeneration(ref GnnModel gnnModel);
    }
}