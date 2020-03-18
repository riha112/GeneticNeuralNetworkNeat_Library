using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;

namespace GNNNeatLibrary.Mutations
{
    // Attach this interface to indicate that
    // class can perform mutation on network.
    public interface IMutation
    {
        // Higher the weight then higher the possibility of selecting this mutation
        int GetWeight { get; set; }

        // Called when network is trying to evolve
        // its structure 
        void ApplyMutation(ref NetModel net);
    }
}
