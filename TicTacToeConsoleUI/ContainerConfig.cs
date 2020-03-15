using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace TicTacToeConsoleUI
{
    public static class Bootstrap
    {
        /// <summary>
        /// Configures dependency injection support for project
        /// </summary>
        public static IContainer Configure()
        {
            var builder =new ContainerBuilder();
            
            // Registers dependencies:
            builder.RegisterType<Application>().As<IApplication>();



            return builder.Build();
        }
    }
}
