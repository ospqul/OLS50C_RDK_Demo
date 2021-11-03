using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OLS50C_RDK_Demo
{
    public class ClientModel : IClientModel
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public ClientModel()
        {
            _client = new TcpClient(AddressFamily.InterNetwork);
            _client.ReceiveTimeout = 500;
            _client.SendTimeout = 500;
        }

        public void Connect(string ip, int port)
        {
            Console.WriteLine($"Connecting to { ip }:{ port }...");
            _client.Connect(ip, port);
            _stream = _client.GetStream();
        }

        public void Close()
        {
            Console.WriteLine($"Closing connection...");
            _stream.Close();
            _stream = null;
            _client.Close();
            _client = null;
        }

        /// <summary>
        /// 
        /// CR+LF are used as end of the command.
        /// More details are available in "Communication Command Formats" in "OLS5000 RDK User's Manual"
        /// 
        /// </summary>
        /// <param name="command"></param>
        public void Write(string command)
        {
            Console.WriteLine($"[Send] { command }");
            var cmd = Encoding.UTF8.GetBytes(command + '\r' + '\n');
            _stream.Write(cmd, 0, cmd.Length);
        }

        public string Receive()
        {
            string message = "";
            int number = _client.Available;
            if (number > 0)
            {
                byte[] buffer = new byte[number];
                var received = _stream.Read(buffer, 0, number);
                message = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine($"[Receive] { message }");
            }
            return message;
        }
    }
}
