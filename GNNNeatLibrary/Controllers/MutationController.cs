using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using GNNNeatLibrary.Mutations;
using GNNNeatLibrary.Utilities;

namespace GNNNeatLibrary.Controllers
{
    public class MutationController
    {
        private IMutationLibrary _mutationLibrary;

        public MutationController(IMutationLibrary mutationLibrary) =>
            _mutationLibrary = mutationLibrary;

        /// <summary>
        /// Applies random mutation to targeted network
        /// </summary>
        /// <param name="target">Network to mutate</param>
        public void Mutate(ref NetModel target)
        {
            var mutation = _mutationLibrary.GetRandomMutationBasedOnWeight();
            mutation.ApplyMutation(ref target);

            // TODO: Test if mutation didn't break the network
        }
    }
}
