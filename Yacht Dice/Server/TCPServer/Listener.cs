using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Service.TCP;

namespace Service.TCP.Server
{
	public class Listener
	{
		SocketAsyncEventArgs m_acceptArgs;
		Socket m_listenSocket;
		AutoResetEvent m_flowControlEvent;
		bool m_threadLive { get; set; }

		public delegate void NewClientHandler(Socket clientSocket, object token);

		public NewClientHandler m_callbackOnNewClient;

		public Listener()
		{
			m_callbackOnNewClient = null;
			m_threadLive = true;
		}

		public void Start(string host, int port, int backlog)
		{
			m_listenSocket = new Socket(
				AddressFamily.InterNetwork,
				SocketType.Stream,
				ProtocolType.Tcp);
			m_listenSocket.NoDelay = true;

			IPAddress address;

			if (host == "0.0.0.0")
			{
				address = IPAddress.Any;
			}
			else
			{
				address =IPAddress.Parse(host); 
			}
			
			IPEndPoint ep = new IPEndPoint(address, port);

			try
			{
				m_listenSocket.Bind(ep);
				m_listenSocket.Listen(backlog);

				m_acceptArgs = new SocketAsyncEventArgs();
				m_acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(onAcceptCompleted);

				Thread listenThread = new Thread(DoListen);
				listenThread.Start();
			}
			catch (Exception e)
			{
				LogManager.Critical(e.Message);
			}
		}

		void DoListen()
		{
			m_flowControlEvent = new AutoResetEvent(false);

			while (m_threadLive)
			{
				m_acceptArgs.AcceptSocket = null;
				bool pending = true;

				try
				{
					pending = m_listenSocket.AcceptAsync(m_acceptArgs);
				}
				catch (Exception e)
				{
					LogManager.Critical(e.Message);
					continue;
				}

				if (!pending)
				{
					onAcceptCompleted(null, m_acceptArgs);
				}

				m_flowControlEvent.WaitOne();
			}
		}

		void onAcceptCompleted(object sender, SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				Socket clientSocket = e.AcceptSocket;

				NetworkManager.Instance.OnNewClient(clientSocket, e);
			}
			else
			{
				LogManager.Warn("Failed to accept client");
			}

			m_flowControlEvent.Set();
		}

		public void Close()
		{
			m_listenSocket.Close();
		}
	}
}