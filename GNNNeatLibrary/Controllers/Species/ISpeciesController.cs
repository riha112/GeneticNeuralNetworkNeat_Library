using DataManager.Models;
using GNNNeatLibrary.Model;

namespace GNNNeatLibrary.Controllers.Species
{
    public interface ISpeciesController
    {
        SpeciesModel GetNew();
        SpeciesModel GetNewWithHead(NetModel headNetModel);
        void CalculateAverageScore(SpeciesModel speciesModel);
        void SortByFitnessScores(SpeciesModel speciesModel);
        NetModel GetRandomNetModel(SpeciesModel speciesModel);

        /// <summary>
        /// Calculates difference between two networks, to check if
        /// they are in the same species.
        /// </summary>
        /// <param name="left">Network one</param>
        /// <param name="right">Network two</param>
        /// <returns></returns>
        bool IsSameFamily(NetModel left, NetModel right);
    }
}