﻿using DeathStarImperator.Core;
using StructureMap;
using StructureMap.Graph;

namespace DeathStarImperator.WebJob
{
    class ContainerInitializer
    {
        public Container Initialize()
        {
            var initialize = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.TheCallingAssembly();
                    scanner.LookForRegistries();
                    scanner.WithDefaultConventions();
                });

                cfg.For<IHubClient>().Use<HubClient>().Singleton();
                cfg.For<Imperator>().Use<Imperator>().Singleton();
                
                cfg.For<ResourceCreator>().Use<ResourceCreator>();
                cfg.For<ResourceAdjuster>().Use<ResourceAdjuster>();
                cfg.For<GlobalTimer>().Use<GlobalTimer>();
                cfg.For<JobProcessor>().Use<JobProcessor>();
                cfg.For<JobSpawner>().Singleton().Use<JobSpawner>();               
            });

            StaticContainer = initialize;
            return initialize;
        }

        /// <summary>
        /// This could be read from configuration
        /// </summary>
        public static Container StaticContainer { get; set; }
    }

}
