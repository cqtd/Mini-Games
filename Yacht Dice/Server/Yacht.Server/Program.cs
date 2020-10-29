using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Yacht.Server
{
	public class Program
	{
		static void Main(string[] args)
		{
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			
			IPEndPoint ep = new IPEndPoint(IPAddress.Any, 7070);
			socket.Bind(ep);
			
			socket.Listen(10);

			Socket clientSock = socket.Accept();
			byte[] buffer = new byte[8192];

			while (!Console.KeyAvailable)
			{
				int n = clientSock.Receive(buffer);

				string data = Encoding.UTF8.GetString(buffer, 0, n);
				Console.WriteLine(data);

				clientSock.Send(buffer, 0, n, SocketFlags.None);
			}
			
			clientSock.Close();
			socket.Close();
		}
	}
}