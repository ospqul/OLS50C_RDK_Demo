using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OLS50C_RDK_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            // Connect RDK Server
            var client = new ClientModel();
            client.Connect("192.168.0.1", 50100);

            // Init Device
            var device = new DeviceModel(client);

            // Get Status            
            var status = device.CheckStatus("GETSTS?", cts.Token);

            // Requesting Connection
            var result = device.SendCommand("CONNECT= 0", cts.Token);
            if (!result)
            {
                return;
            }
            status = device.CheckStatus("GETSTS?", cts.Token);

            // Log in
            result = device.SendCommand("INITNRML= TMELD,olympus", cts.Token);
            if (!result)
            {
                return;
            }
            status = device.CheckStatus("GETSTS?", cts.Token);

            // Move out stage
            Console.WriteLine("=============================================");
            Console.WriteLine("  DANGER!!!");
            Console.WriteLine("=============================================");
            Console.WriteLine("Do you want to move out the stage? (y/n):");
            var input = Console.ReadLine();
            if (input == "y")
            {
                device.SendCommand("MVSTG= -50000,50000", cts.Token);
            }
            status = device.CheckStatus("GETSTS?", cts.Token);

            // Move in stage
            Console.WriteLine("Do you want to move in the stage? (y/n):");
            input = Console.ReadLine();
            if (input == "y")
            {
                device.SendCommand("MVSTG= 0,0", cts.Token);
            }
            status = device.CheckStatus("GETSTS?", cts.Token);

            // Get current Lens and Zoom
            status = device.CheckStatus("GETOB?", cts.Token);
            Console.WriteLine($"OB,Zoom: { status }");

            // Switch Lens
            Console.WriteLine("=============================================");
            Console.WriteLine("  DANGER!!!");
            Console.WriteLine("=============================================");
            Console.WriteLine("Which Lens do you want to select? (1/2/3/4/5, others to skip):");
            input = Console.ReadLine();            
            if ((input == "1")
                || (input == "2")
                || (input == "3")
                || (input == "4")
                || (input == "5"))
            {
                device.SendCommand($"CHOB= { input }", cts.Token);
                status = device.CheckStatus("GETOB?", cts.Token);
                Console.WriteLine($"OB,Zoom: { status }");
            }
            status = device.CheckStatus("GETSTS?", cts.Token);

            // Switch Zoom           
            Console.WriteLine("Which Zoom do you want to select? (10/12/15/20/30/40/60/80, others to skip):");
            input = Console.ReadLine();
            if ((input == "10")
                || (input == "12")
                || (input == "15")
                || (input == "20")
                || (input == "30")
                || (input == "40")
                || (input == "60")
                || (input == "80"))
            {
                device.SendCommand($"CHZOOM= { input }", cts.Token);
                status = device.CheckStatus("GETOB?", cts.Token);
                Console.WriteLine($"OB,Zoom: { status }");
            }
            status = device.CheckStatus("GETSTS?", cts.Token);

            // Load Macro
            Console.WriteLine("Which macro do you want to load? (macro file name):");
            input = Console.ReadLine();
            if (input.EndsWith(".mcr"))
            {
                device.SendCommand($"RDWIZ= { input }", cts.Token);
            }
            else
            {
                Console.WriteLine("macro file should end with .mcr");
            }
            status = device.CheckStatus("GETSTS?", cts.Token);

            // Execute Macro
            // A completion notification is returned after completion of macro.
            Console.WriteLine("Do you want to execute macro? (y/n):");
            input = Console.ReadLine();
            if (input == "y")
            {
                device.SendCommand("WIZEXE= 0", cts.Token);
                Console.WriteLine("Macro is completed!");
            }
            status = device.CheckStatus("GETSTS?", cts.Token);

            // Disconnect
            device.SendCommand("CHMODE= 0", cts.Token);
            device.SendCommand("Exit= 0", cts.Token);
            device.SendCommand("DISCONNECT= 0", cts.Token);
        }
    }
}
