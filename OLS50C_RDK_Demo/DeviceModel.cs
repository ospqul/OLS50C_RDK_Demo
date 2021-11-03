using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OLS50C_RDK_Demo
{
    public class DeviceModel : IDeviceModel
    {
        private readonly IClientModel _client;

        public DeviceModel(IClientModel client)
        {
            _client = client;
        }

        /// <summary>
        /// 
        /// Issue Command.
        /// More details are available in "Communication Command Formats" in "OLS5000 RDK User's Manual"
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool SendCommand(string command, CancellationToken token)
        {
            bool result = false;

            // Verify if command is in correct format
            if (!command.Contains("= "))
            {
                Console.WriteLine($"SendCommand Wrong Format: { command }");
                return false;
            }

            string name = command.Split('=')[0];

            // Send command to RDK Server
            _client.Write(command);

            // Wait to receive reply from RDK server
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"Command Cancelled: { command }");
                    result = false;
                    break;
                }

                var message = _client.Receive();
                if (message.StartsWith($"{ name }: +"))
                {
                    Console.WriteLine($"Command Success: { command }");
                    result = true;
                    break;
                }
                else if (message.StartsWith($"{ name }: !"))
                {
                    Console.WriteLine($"Command Failure: { command }");
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// Check status.
        /// More details are available in "Communication Command Formats" in "OLS5000 RDK User's Manual"
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string CheckStatus(string command, CancellationToken token)
        {
            string result = "";

            // Verify if command is in correct format
            if (!command.Contains("?"))
            {
                Console.WriteLine($"CheckStatus Wrong Format: { command }");
                return "";
            }

            string name = command.Split('?')[0];

            // Send command to RDK Server
            _client.Write(command);

            // Wait to receive reply from RDK server
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"Command Cancelled: { command }");
                    result = "";
                    break;
                }

                var message = _client.Receive();
                if (message.StartsWith($"{ name }: "))
                {
                    int startIndex = name.Length + 2;
                    var returnedValue = message.Substring(startIndex);
                    if (returnedValue.StartsWith("!"))
                    {
                        Console.WriteLine($"Command Failure: { command }");
                        result = "";
                    }
                    else
                    {
                        Console.WriteLine($"Command Success: { command }");
                        result = returnedValue;
                    }
                    break;
                }
            }
            return result;
        }


    }
}
