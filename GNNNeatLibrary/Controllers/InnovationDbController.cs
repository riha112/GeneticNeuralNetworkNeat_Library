using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Processors;

namespace GNNNeatLibrary.Controllers
{
    public class InnovationDbController : IInnovationController
    {
        private readonly IInnovationProcessor _innovationProcessor;

        public InnovationDbController(IInnovationProcessor innovationProcessor) =>
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
                return innovation;
            }
        }
    }
}
