using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace CQ.MiniGames
{
	public class Client : MonoBehaviour
	{
		string cmd;

		Queue<string> queue;
		
		
		void Awake()
		{
			queue = new Queue<string>();
		}

		void Start()
		{
			Task.Run(Connect);
		}

		public void Log(string msg)
		{
			queue.Enqueue(msg);
		}

		void Update()
		{
			lock (queue)
			{
				while (queue.Count > 0)
				{
					Debug.Log(queue.Dequeue());
				}
			}
		}

		async Task Connect()
		{
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			
			var ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7070);
			socket.Connect(ep);

			cmd = string.Empty;
			byte[] receiverBuffer = new byte[8192];
			
			Log("Connected... Enter Q to exit");
			
			ulong current = UInt64.MinValue;
			while (current != uint.MaxValue)
			{
				current++;
				
				byte[] buffer = Encoding.UTF8.GetBytes(cmd);
				socket.Send(buffer, SocketFlags.None);

				int n = socket.Receive(receiverBuffer);
				string data = Encoding.UTF8.GetString(receiverBuffer, 0, n);
				
				Log(data);
			}

			await Task.Delay(1);
				
			socket.Close();
		}
	}
}