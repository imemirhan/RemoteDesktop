using System;
using System.Windows.Forms;
using Client.Network;

namespace Client
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("[Client] Starting viewer...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Form
            {
                Text = "Remote Viewer",
                Width = 1024,
                Height = 768
            };

            var pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            form.Controls.Add(pictureBox);

            string hostIp = Microsoft.VisualBasic.Interaction.InputBox("Enter Host IP:", "Connect", "127.0.0.1");

            var viewer = new ScreenViewer(pictureBox, hostIp);
            viewer.Start();

            Application.Run(form);
        }
    }
}