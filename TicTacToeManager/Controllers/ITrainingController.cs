using GNNNeatLibrary.Model;

namespace TicTacToeManager.Controllers
{
    public interface ITrainingController
    {
        void TrainGeneration(GnnModel model);
    }
}