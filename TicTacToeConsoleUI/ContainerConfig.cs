using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;

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

            // Initializes DataManage/Models
            //builder.RegisterAssemblyTypes(Assembly.Load(nameof(DataManager)))
            //    .Where(t => t.Namespace.Contains("Models"))
            //    .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));


            return builder.Build();
        }
    }
}
