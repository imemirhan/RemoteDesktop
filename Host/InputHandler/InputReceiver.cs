using System;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Shared.Models;
using System.Runtime.InteropServices;

namespace Host.InputHandler
{
    public class InputReceiver
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;

        public InputReceiver(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();
        }

        public async Task ListenAsync()
        {
            while (true)
            {
                // Read length prefix (4 bytes)
                var lengthBytes = new byte[4];
                int read = await _stream.ReadAsync(lengthBytes, 0, 4);
                if (read == 0) break;
                int length = BitConverter.ToInt32(lengthBytes, 0);

                // Read payload
                var buffer = new byte[length];
                int totalRead = 0;
                while (totalRead < length)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, totalRead, length - totalRead);
                    if (bytesRead == 0) break;
                    totalRead += bytesRead;
                }

                var json = System.Text.Encoding.UTF8.GetString(buffer);
                var command = JsonSerializer.Deserialize<InputCommand>(json);

                if (command != null)
                    SimulateInput(command);
            }
        }

        private void SimulateInput(InputCommand cmd)
        {
            // Use Windows API SendInput here to simulate mouse & keyboard events
            // I can provide this code next if you want
        }
    }
}