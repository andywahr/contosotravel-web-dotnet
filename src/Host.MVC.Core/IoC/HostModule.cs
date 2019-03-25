using Autofac;
using ContosoTravel.Web.Application.Data;
using ContosoTravel.Web.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoTravel.Web.Host.MVC.Core.IoC
{
    public class HostModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ASPNetCoreCartCookieProvider>().As<ICartCookieProvider>();
            switch (Application.ContosoConfiguration.DataType)
            {
                case Application.DataType.SQL:
                    builder.RegisterType<SQLServerConnectionProvider>().As<ISQLConnectionProvider>();
                    break;

                case Application.DataType.MySQL:
                    builder.RegisterType<Application.Data.MySQL.MySQLProvider>().As<ISQLConnectionProvider>();
                    break;
            }
        }
    }
}
