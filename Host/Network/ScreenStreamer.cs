using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Host.Network
{
    public class ScreenStreamer
    {
        private TcpListener _listener;
        private bool _isRunning = false;
        private readonly int _port;

        public ScreenStreamer(int port = 9999)
        {
            _port = port;
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            _isRunning = true;

            Console.WriteLine($"[Host] Listening on port {_port}...");

            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (_isRunning)
                {
                    try
                    {
                        var client = _listener.AcceptTcpClient();
                        Console.WriteLine("[Host] Client connected.");
                        HandleClient(client);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Host] Error: {ex.Message}");
                    }
                }
            });
        }

        private void HandleClient(TcpClient client)
        {
            using var stream = client.GetStream();
            var screenSize = Screen.PrimaryScreen.Bounds.Size;

            while (_isRunning && client.Connected)
            {
                try
                {
                    using var bmp = new Bitmap(screenSize.Width, screenSize.Height);
                    using var g = Graphics.FromImage(bmp);
                    g.CopyFromScreen(Point.Empty, Point.Empty, screenSize);

                    using var ms = new MemoryStream();
                    bmp.Save(ms, ImageFormat.Jpeg);
                    var imgBytes = ms.ToArray();

                    var lengthBytes = BitConverter.GetBytes(imgBytes.Length);
                    stream.Write(lengthBytes, 0, 4);
                    stream.Write(imgBytes, 0, imgBytes.Length);

                    Thread.Sleep(100); // ~10 FPS
                }
                catch
                {
                    Console.WriteLine("[Host] Connection dropped.");
                    break;
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();
        }
    }
}
