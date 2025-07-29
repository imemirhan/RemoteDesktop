using Host.Network;

namespace Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Host] Remote Desktop Host starting...");

            var streamer = new ScreenStreamer(9999);
            streamer.Start();

            Console.WriteLine("[Host] Press Enter to exit.");
            Console.ReadLine();

            streamer.Stop();
        }
    }
}