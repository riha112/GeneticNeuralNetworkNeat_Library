using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GNNNeatLibrary.Controllers;
using GNNNeatLibrary.Controllers.Net;
using GNNNeatLibrary.Model;

namespace TicTacToeManager.Controllers
{
    public class TrainingController : ITrainingController
    {
        private readonly IGnnController _gnnController;
        private readonly INetNeatController _netNeatController;

        public TrainingController(IGnnController gnnController, INetNeatController netNeatController)
        {
            _gnnController = gnnController;
            _netNeatController = netNeatController;
        }

        public void TrainGeneration(GnnModel model)
        {
            _gnnController.SpecifyNetworks(ref model);
            for (var i = 0; i < model.ActiveBatchModel.Networks.Count - 1; i++)
            {
                var currentNet = model.ActiveBatchModel.Networks[i];
                for (var x = i + 1; x < model.ActiveBatchModel.Networks.Count; x++)
                {
                    var compNet = model.ActiveBatchModel.Networks[x];
                    
                    var gameOne = new GameController(_netNeatController, currentNet, compNet);
                    gameOne.DoMove();

                    var gameTwo = new GameController(_netNeatController, compNet, currentNet);
                    gameOne.DoMove();

                    var gameOneResult = gameOne.CheckGrid();
                    var gameTwoResult = gameTwo.CheckGrid();

                    if (gameOneResult == 1 || gameTwoResult == -1) {
                        currentNet.FitnessScore++;
                        compNet.FitnessScore--;
                    }
                    
                    if (gameOneResult == -1 || gameTwoResult == 1)
                    {
                        currentNet.FitnessScore--;
                        compNet.FitnessScore++;
                    }
                }
            }
            _gnnController.EvaluateNetworks(ref model);
            _gnnController.PopulateNextGeneration(ref model);
        }
    }
}
