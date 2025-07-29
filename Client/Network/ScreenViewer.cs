using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Client.Network
{
    public class ScreenViewer
    {
        private readonly PictureBox _pictureBox;
        private readonly string _hostIp;
        private readonly int _port;

        public ScreenViewer(PictureBox pictureBox, string hostIp, int port = 9999)
        {
            _pictureBox = pictureBox;
            _hostIp = hostIp;
            _port = port;
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    using var client = new TcpClient(_hostIp, _port);
                    using var stream = client.GetStream();

                    while (true)
                    {
                        byte[] lengthBytes = new byte[4];
                        if (stream.Read(lengthBytes, 0, 4) != 4)
                            break;

                        int length = BitConverter.ToInt32(lengthBytes, 0);
                        byte[] imgBytes = new byte[length];
                        int read = 0;
                        while (read < length)
                            read += stream.Read(imgBytes, read, length - read);

                        using var ms = new MemoryStream(imgBytes);
                        var bmp = new Bitmap(ms);

                        _pictureBox.Invoke(new Action(() =>
                        {
                            _pictureBox.Image?.Dispose();
                            _pictureBox.Image = new Bitmap(bmp);
                        }));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"[Client] Error: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }
    }
}