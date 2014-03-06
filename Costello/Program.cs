using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Costello
{
    class Program
    {
        private static IEnumerator lines;
        private static TcpClient client;

        static void Main(string[] args)
        {
            lines = File.ReadAllLines("Script.txt").GetEnumerator();
            
            client = new TcpClient();
            client.Connect(IPAddress.Loopback, 8091);
            var stream = client.GetStream();
            int i;
            var readBuffer = new byte[256];
            while ((i = stream.Read(readBuffer, 0, readBuffer.Length)) != 0)
            {
                lines.MoveNext();
                var readSoFar = Encoding.ASCII.GetString(readBuffer, 0, i);
                if (readSoFar == (string) lines.Current)
                {
                    if (!lines.MoveNext()) break;

                    Thread.Sleep(1000);
                    var writeBuffer = Encoding.ASCII.GetBytes((string)lines.Current);
                    Console.WriteLine((string)lines.Current);
                    stream.Write(writeBuffer, 0, writeBuffer.Length);
                }
            }

            Console.ReadLine();
        }        
    }
}
