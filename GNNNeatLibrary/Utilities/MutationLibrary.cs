using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GNNNeatLibrary.Mutations;
using System.Linq;

namespace GNNNeatLibrary.Utilities
{
    public class MutationLibrary : IMutationLibrary
    {
        private static List<IMutation> _mutations = new List<IMutation>();
        private static int _totalWeight = 0;

        public MutationLibrary()
        {
            if (_mutations.Count != 0) return;

            // Loads all types form folder Mutations with Assembly
            //var namespacePath = $"{nameof(GNNNeatLibrary)}.{nameof(GNNNeatLibrary.Mutations)}";
            //var mutationTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && t.Namespace == namespacePath);

            //foreach (var type in mutationTypes)
            //{
            //    _mutations.Add((IMutation)Activator.CreateInstance(type));
            //}
            BuildLibrary();

            _mutations = _mutations.OrderByDescending(m => m.GetWeight).ToList();
            _totalWeight = (int)_mutations.Sum(m => m.GetWeight);
        }

        private static void BuildLibrary()
        {
            _mutations.Add(new AddNodeMutation());
        }

        public IMutation GetRandomMutationBasedOnWeight()
        {
            var rnd = new Random();
            var selected = rnd.Next(0, _totalWeight);
            var currentSum = 0;

            for (var i = 0; i < _mutations.Count - 1; i++)
            {
                currentSum += _mutations[i].GetWeight;
                if (selected <= currentSum)
                    return _mutations[i];
            }

            return _mutations.Last();
        }

        public IMutation GetRandomMutation()
        {
            var rnd = new Random();
            return _mutations[rnd.Next(0, _mutations.Count)];
        }

        public IMutation GetMutation(int id)
        {
            if (id < 0 || id > _mutations.Count)
                throw new IndexOutOfRangeException();

            return _mutations[id];
        }
    }
}
