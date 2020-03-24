using System;
using System.Collections.Generic;
using System.Text;

namespace GNNNeatLibrary.Utilities
{
    public static class Config
    {
        public static int InputLayerSize { get; set; } = 10;
        public static int OutputLayerSize { get; set; } = 1;
        public static double SpeciesDistanceThreshold { get; set; } = 2;

        public static double MinimumFitnessScore { get; set; } = 0;
        public static double KeepPerSpecies { get; set; } = 0.25;

        public static uint NetworkCountPerPopulation { get; set; } = 60;

        public static int AddRandomConnectionMutationsOnStartMin { get; set; } = 5;
        public static int AddRandomConnectionMutationsOnStartMax { get; set; } = 15;
    }
}
