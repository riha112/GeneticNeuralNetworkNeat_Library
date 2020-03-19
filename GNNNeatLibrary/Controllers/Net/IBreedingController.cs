using DataManager.Models;

namespace GNNNeatLibrary.Controllers.Net
{
    public interface IBreedingController
    {
        NetModel Breed(BatchModel family, NetModel mother, NetModel father);
    }
}