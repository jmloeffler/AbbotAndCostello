using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Core;

namespace Abbot
{
    class Program
    {
        private static IEnumerator lines;
        private static TcpListener listener;
        
        static void Main(string[] args)
        {
            lines = File.ReadAllLines("Script.txt").GetEnumerator();
            
            listener = new TcpListener(IPAddress.Loopback, 8091);
            listener.Start();
            listener.BeginAcceptTcpClient(BeginConnect, new TcpSessionState{ Connection = listener, Line = lines});

            Console.ReadLine();
            listener.Stop();
        }

        private static void BeginConnect(IAsyncResult ar)
        {
            var state = (TcpSessionState) ar.AsyncState;
            var client = state.Connection.EndAcceptTcpClient(ar);
            var stream = client.GetStream();

            while (client.Connected && state.Line.MoveNext())
            {
                Thread.Sleep(1000);
                var lineBuffer = Encoding.ASCII.GetBytes((string)state.Line.Current);
                Console.WriteLine((string)state.Line.Current);
                stream.Write(lineBuffer, 0, lineBuffer.Length);

                if (!state.Line.MoveNext()) break;

                var readBuffer = new byte[256];
                int i;
                while ((i = stream.Read(readBuffer, 0, readBuffer.Length)) != 0)
                {
                    var lineSoFar = Encoding.ASCII.GetString(readBuffer, 0, i);
                    if (lineSoFar == (string) state.Line.Current)
                    {
                        break;
                    }
                    
                }
            }
        }
    }
}
