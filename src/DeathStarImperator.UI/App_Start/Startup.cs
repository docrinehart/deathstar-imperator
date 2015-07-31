using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Owin;
using Microsoft.Owin;
using StructureMap;
using IDependencyResolver = Microsoft.AspNet.SignalR.IDependencyResolver;

[assembly: OwinStartup(typeof(DeathStarImperator.UI.Startup))]
namespace DeathStarImperator.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure StructureMap for SignalR
            GlobalHost.DependencyResolver = DependencyResolver.Current.GetService<IDependencyResolver>();

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }

    public class StructureMapSignalRDependencyResolver : DefaultDependencyResolver
    {
        private readonly IContainer _container;

        public StructureMapSignalRDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            return _container.TryGetInstance(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var objects = _container.GetAllInstances(serviceType).Cast<object>();
            return objects.Concat(base.GetServices(serviceType));
        }
    }
}