using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataManager.Models;
using GNNNeatLibrary.Controllers.Net;
using GNNNeatLibrary.Controllers.Species;
using GNNNeatLibrary.Model;
using GNNNeatLibrary.Utilities;

namespace GNNNeatLibrary.Controllers
{
    public class GnnController : IGnnController
    {
        private readonly ISpeciesController _speciesController;
        private readonly IBreedingController _breedingController;
        private readonly IMutationController _mutationController;
        private readonly IBatchController _batchController;
        private readonly INetController _netController;

        public GnnController(ISpeciesController speciesController, 
                             IBreedingController breedingController, 
                             IMutationController mutationController, 
                             IBatchController batchController, 
                             INetController netController)
        {
            _speciesController = speciesController;
            _breedingController = breedingController;
            _mutationController = mutationController;
            _batchController = batchController;
            _netController = netController;
        }

        public GnnModel Load(int batchId) => 
            new GnnModel {ActiveBatchModel = _batchController.Load(batchId)};

        public GnnModel New(string name, string description) => 
            new GnnModel() {ActiveBatchModel = _batchController.New(name, description)};

        /// <summary>
        /// From unsigned network list fetches all networks and splits them into
        /// families/species, witch is done by using Neat network similarity algorithm.
        /// </summary>
        /// <param name="gnnModel">Network to specify</param>
        public void SpecifyNetworks(ref GnnModel gnnModel)
        {
            // If nothing assign reloads network
            if(gnnModel.UnsignedNetworks.Count == 0)
               gnnModel.UnsignedNetworks = new List<NetModel>(gnnModel.ActiveBatchModel.Networks);
            
            gnnModel.Species = new List<SpeciesModel>();
            foreach (var network in gnnModel.UnsignedNetworks)
            {
                // Checks if current network is in the same family/specie as
                // already existing ones
                var isNewSpecies = true;
                foreach (var specie in gnnModel.Species)
                {
                    if (_speciesController.IsSameFamily(network, specie.Head))
                    {
                        specie.Members.Add(network);
                        isNewSpecies = false;
                        break;
                    }
                }
                // If its not then new specie is created, and its head is assigned as current network,
                // so that later we can compare species.Hed with other networks to determine its relationship 
                // to specie.
                if(isNewSpecies)
                    gnnModel.Species.Add(_speciesController.GetNewWithHead(network));
            }

            gnnModel.UnsignedNetworks.Clear();
        }

        /// <summary>
        /// Sums all species average fitness scores.
        /// And assigns value to gnnModel.FullFitnessScore
        /// </summary>
        /// <param name="gnnModel"></param>
        private static void CalculateFullFitnessScore(ref GnnModel gnnModel)
        {
            gnnModel.FullFitnessScore = gnnModel.Species.Sum(s => s.AverageFitnessScore);
        }

        private static void SortSpeciesByAverageFitnessScore(ref GnnModel gnnModel)
        {
            gnnModel.Species = gnnModel.Species.OrderBy(s => s.AverageFitnessScore).ToList();
        }

        public void EvaluateNetworks(ref GnnModel gnnModel)
        {
            foreach (var specie in gnnModel.Species)
            {
                _speciesController.CalculateAverageScore(specie);
                _speciesController.SortByFitnessScores(specie);
            }
            SortSpeciesByAverageFitnessScore(ref gnnModel);
            CalculateFullFitnessScore(ref gnnModel);
        }

        public void PopulateNextGeneration(ref GnnModel gnnModel)
        {
            gnnModel.ActiveBatchModel.Generation++;
            gnnModel.UnsignedNetworks = new List<NetModel>();

            #region Adds survivors to next generation

            // Adds survivors of each specie to the next generation
            foreach (var specie in gnnModel.Species)
            {
                // Calculates how many networks keep
                var keepAlive = (int)(specie.Members.Count * Config.KeepPerSpecies);

                // We want at-least one survivor per specie.
                if (keepAlive <= 0)
                    keepAlive = 1;

                // Ads them to next gen.
                for (var i = 0; i < keepAlive; i++)
                    gnnModel.UnsignedNetworks.Add(specie.Members[i]);

                // Disables old networks
                for (var i = keepAlive; i < specie.Members.Count; i++)
                {
                    specie.Members[i].Enabled = false;
                    _netController.Save(specie.Members[i]);
                }
            }
            #endregion

            #region Populates empty spaces with offspings

            // Populates rest of generation with offspring of survivors
            var networksToAdd = Config.NetworkCountPerPopulation - gnnModel.UnsignedNetworks.Count;
            for (var i = 0; i < networksToAdd; i++)
            {
                var specie = GetRandomSpeciesModel(gnnModel);

                // TODO: Prevent mother to be same as father
                var mother = _speciesController.GetRandomNetModel(specie);
                var father = _speciesController.GetRandomNetModel(specie);

                var child = _breedingController.Breed(gnnModel.ActiveBatchModel, mother, father);

                _mutationController.MutateWithBatch(ref child, 1);  // Add node mutation
                _mutationController.MutateWithBatch(ref child, 2);  // Add connection mutation
                _mutationController.MutateWithBatch(ref child, 3);  // Connections weight mutation


                // Orders
                child.Connections = child.Connections.OrderBy(c => c.InnovationId).ToList();
                child.Nodes = child.Nodes.OrderBy(n => n.InnovationId).ToList();
                gnnModel.UnsignedNetworks.Add(child);
            }
            #endregion
            
            // Rebuilds batch from db (Why? Because innovation table has changed)
            gnnModel.ActiveBatchModel = _batchController.Load(gnnModel.ActiveBatchModel.Id);
        }

        // TODO: Test with negative score
        private static SpeciesModel GetRandomSpeciesModel(GnnModel gnnModel)
        {
            var rnd = new Random();
            var targetRange = rnd.Next(0, (int)Math.Ceiling(gnnModel.FullFitnessScore));
            var currentScoreSum = 0.0;
            for (var i = 0; i < gnnModel.Species.Count - 1; i++)
            {
                currentScoreSum += gnnModel.Species[i].AverageFitnessScore;
                if (targetRange <= currentScoreSum)
                    return gnnModel.Species[i];
            }
            return gnnModel.Species.LastOrDefault();
        }
    }
}
