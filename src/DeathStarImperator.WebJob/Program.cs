using System;
using System.Threading;
using DeathStarImperator.Core;

namespace DeathStarImperator.WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a IPC wait handle with a unique identifier.
            bool createdNew;
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "ImperatorWaitHandle", out createdNew);
            var signaled = false;

            // If the handle was already there, inform the other process to exit itself.
            // Afterwards we'll also die.
            if (!createdNew)
            {
                Console.WriteLine("Found other Imperator process. Requesting stop...");
                waitHandle.Set();
                Console.WriteLine("Informer exited.");

                return;
            }


            Console.WriteLine("Initializing Container...");
            var container = new ContainerInitializer().Initialize();

            Console.WriteLine("Connecting to UI...");
            var hubClient = container.GetInstance<IHubClient>();
            hubClient.OpenConnection();
            Console.WriteLine("Connected");

            Console.WriteLine("Initializing Imperator...");
            var i = container.GetInstance<Imperator>();
            i.InitializeConfig();
            i.StartImperator();
           
            waitHandle.WaitOne();
            Console.WriteLine("Closing Imperator...");
        }

    }
}
