using System;
using System.Linq;
using DataManager.Models;
using GNNNeatLibrary.Controllers.Net;
using GNNNeatLibrary.Model;
using GNNNeatLibrary.Utilities;

namespace GNNNeatLibrary.Controllers.Species
{
    public class SpeciesController : ISpeciesController
    {
        private readonly INetNeatController _netNeatController;

        public SpeciesController(INetNeatController netNeatController) =>
            _netNeatController = netNeatController;

        public SpeciesModel GetNew()
        {
            return new SpeciesModel();
        }

        public SpeciesModel GetNewWithHead(NetModel headNetModel)
        {
            var species = new SpeciesModel { Head = headNetModel };
            species.Members.Add(headNetModel);
            return species;
        }

        public void CalculateAverageScore(SpeciesModel speciesModel)
        {
            speciesModel.AverageFitnessScore = speciesModel.Members.Average(s => s.FitnessScore);
        }

        public void SortByFitnessScores(SpeciesModel speciesModel)
        {
            speciesModel.Members = speciesModel.Members.OrderByDescending(x => x.FitnessScore).ToList();
        }

        public NetModel GetRandomNetModel(SpeciesModel speciesModel)
        {
            var rnd = new Random();
            var max = (int) Math.Ceiling(speciesModel.Members.Sum(m => m.FitnessScore));
            var targetRange = rnd.Next(0, max);
            var currentScoreSum = 0.0;
            for (var i = 0; i < speciesModel.Members.Count - 1; i++)
            {
                currentScoreSum += speciesModel.Members[i].FitnessScore;
                if (targetRange <= currentScoreSum)
                    return speciesModel.Members[i];
            }
            return speciesModel.Members.LastOrDefault();
        }

        /// <summary>
        /// Calculates difference between two networks, to check if
        /// they are in the same species.
        /// </summary>
        /// <param name="left">Network one</param>
        /// <param name="right">Network two</param>
        /// <returns></returns>
        public bool IsSameFamily(NetModel left, NetModel right)
        {
            /* If both network have no connection this means that they will
             * always do the same amount of work (more precisely no work at all),
             * thus you can assume that network are in the same family/species.
             */
            if (left.Connections.Count == 0 && right.Connections.Count == 0)
                return true;

            /* To calculate distance between two networks we use NEAT formula:
             * δ = E/N + D/N + W/M + U/N
             * N - count of connections in the largest right network
             * D - Disjoint connections
             * E - Excess connection count
             * W - Difference in weights for same connections
             * M - Count of same connections
             * U - Count of differences in disabled connections
             * For more information on what is Disjoint and Excess look at "PAGE 12" in:
             * http://nn.cs.utexas.edu/downloads/papers/stanley.ec02.pdf
             */

            // From formula we ensure that right network always have the most
            // connections, thus setting N as right.Connection.Count
            if (left.Connections.Count > right.Connections.Count)
                return IsSameFamily( right,  left);


            var N = right.Connections.Count;
            if (N < 10) N = 3; // Spreads networks into more species if they are small.

            // Each connection has an innovation id, thus we can
            // Create array where each connection is stored in cell
            // based on its innovations id.
            (bool hasInnovation, double weight, bool isEnabled)[,] table;

            // To know how large table to make we get the ranges for both innovations
            // and combine them into one
            var rangeLeft = _netNeatController.GetInnovationRange(left);
            var rangeRight = _netNeatController.GetInnovationRange(right);
            (int min, int max) range = (
                rangeLeft.min < rangeRight.min ? rangeLeft.min : rangeRight.min,
                rangeLeft.max > rangeRight.max ? rangeLeft.max : rangeRight.max
            );

            // Populate table: if has innovation then true else false
            table = new (bool, double, bool)[2, range.max - range.min + 1];
            var nets = new NetModel[2] { left, right };
            for (var n = 0; n < 2; n++)
            {
                foreach (var connection in nets[n].Connections)
                {
                    var column = connection.InnovationId - range.min;
                    table[n, column] = (true, connection.Weight, connection.Enabled);
                }
            }

            // Calculates: Disjoints (D), Excesses(E), Weight difference(W),
            // Same connections(M) and Difference in disabled connections (U)
            var (D, E, W, M, U) = (0, 0, 0.0, 0, 0);

            // Start from end because its the simplest way of finding excesses
            var isExcess = true;
            var nonExcessRow = range.max == rangeLeft.max ? 1 : 0;
            for (var i = range.max - range.min; i >= 0; i--)
            {
                // If both are empty skip
                if (!table[0, i].hasInnovation && !table[1, i].hasInnovation)
                    continue;

                #region Excess calculation
                if (isExcess)
                {
                    // If still not joined or disjointed
                    if (!table[nonExcessRow, i].hasInnovation)
                    {
                        E++;
                        continue;
                    }
                    isExcess = false;
                }
                #endregion

                #region Disjoint calculation
                if (table[0, i].hasInnovation != table[1, i].hasInnovation)
                {
                    D++;
                    continue;
                }
                #endregion

                #region Same connection callculation
                if (table[0, i].isEnabled != table[1, i].isEnabled)
                    U++;

                M++;
                W += Math.Abs(table[0, i].weight - table[0, 1].weight);
                #endregion
            }

            var distance = (double)E / N + (double)D / N + (double)U / N + W / M;

            return distance <= Config.SpeciesDistanceThreshold;
        }
    }
}
