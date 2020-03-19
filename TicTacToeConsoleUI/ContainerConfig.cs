using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using DataManager.Utilities;
using GNNNeatLibrary.Controllers;
using GNNNeatLibrary.Controllers.Innovations;
using GNNNeatLibrary.Utilities;
using TicTacToeManager.Training;

namespace TicTacToeConsoleUI
{
    public static class ContainerConfig
    {
        /// <summary>
        /// Configures dependency injection support for project
        /// </summary>
        public static IContainer Configure()
        {
            var builder =new ContainerBuilder();
            
            // Registers dependencies:
            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<SqlDataAccess>().As<ISqlDataAccess>();

            // Initializes DataManager/Processors
            builder.RegisterAssemblyTypes(Assembly.Load(nameof(DataManager)))
                .Where(t => t.Namespace.Contains(nameof(DataManager.Processors)))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterType<MutationLibrary>().As<IMutationLibrary>();
            builder.RegisterType<InnovationController>().As<IInnovationController>();

            // Initializes GNNNeatLibrary/Controllers/Net
            builder.RegisterAssemblyTypes(Assembly.Load(nameof(GNNNeatLibrary)))
                .Where(t => t.Namespace.Contains("Controllers.Net"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));


            // Initializes GNNNeatLibrary/Controllers/Species
            builder.RegisterAssemblyTypes(Assembly.Load(nameof(GNNNeatLibrary)))
                .Where(t => t.Namespace.Contains("Controllers.Species"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterType<BatchController>().As<IBatchController>();
            builder.RegisterType<GnnController>().As<IGnnController>();

            builder.RegisterType<GameTrainer>().As<IGameTrainer>();


            return builder.Build();
        }
    }
}
