using System;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Shared.Models;
using System.Windows.Forms;

namespace Client.Connectors
{
    public class InputSender
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;

        public InputSender(string hostIp, int port = 9999)
        {
            _client = new TcpClient(hostIp, port);
            _stream = _client.GetStream();
        }

        public async Task SendInputAsync(InputCommand command)
        {
            var json = JsonSerializer.Serialize(command);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);

            // Send length prefix
            var lengthBytes = BitConverter.GetBytes(bytes.Length);
            await _stream.WriteAsync(lengthBytes, 0, lengthBytes.Length);

            // Send payload
            await _stream.WriteAsync(bytes, 0, bytes.Length);
        }

        public void Close()
        {
            _stream.Close();
            _client.Close();
        }
    }
}