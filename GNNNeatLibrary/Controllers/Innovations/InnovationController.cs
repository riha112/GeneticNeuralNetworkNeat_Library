using System;
using DataManager.Models;
using DataManager.Processors;

namespace GNNNeatLibrary.Controllers.Innovations
{
    public class InnovationController : IInnovationController
    {
        private readonly IInnovationProcessor _innovationProcessor;

        public InnovationController(IInnovationProcessor innovationProcessor) =>
            _innovationProcessor = innovationProcessor;

        /// <summary>
        /// If innovation exists in database then it's returned, otherwise
        /// new innovation is created and returned. 
        /// </summary>
        /// <param name="innovation">Fetch-able innovation</param>
        /// <returns>Linked innovation</returns>
        public InnovationModel FetchWithInsertOnFail(InnovationModel innovation)
        {
            try
            {
                // Searches for innovation
                var output = _innovationProcessor.Find(
                    innovation.Type, 
                    innovation.NodeFromId, 
                    innovation.NodeToId, 
                    innovation.BatchId);
                return output;
            }
            catch (Exception)   // TODO: Make custom Exception for element not found
            {
                // If innovation is not found then save it
                _innovationProcessor.Save(ref innovation);  // Saves and Updates Id
                // Should add to batch
                return innovation;
            }
        }
    }
}
