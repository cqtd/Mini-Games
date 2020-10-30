using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Service.TCP;

namespace Service.TCP.Client
{
	public class Network
	{
		Socket m_socket;

		public Network()
		{
			
		}

		public void Connect(string address, int port)
		{
			m_socket = new Socket(
				AddressFamily.InterNetwork,
				SocketType.Stream,
				ProtocolType.Tcp);

			m_socket.NoDelay = true;
			
			IPEndPoint ep = new IPEndPoint(IPAddress.Parse(address), port);
			
			SocketAsyncEventArgs eventArgs = new SocketAsyncEventArgs();
			eventArgs.Completed += onConnected;
			eventArgs.RemoteEndPoint = ep;

			bool pending = m_socket.ConnectAsync(eventArgs);

			if (!pending)
			{
				onConnected(null, eventArgs);
			}
		}

		void onConnected(object sender, SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				LogManager.Success("소켓 연결 됨");
			}
			else
			{
				LogManager.Warn(e.SocketError.ToString());
			}
		}

		public void Send(Packet packet)
		{
			if (m_socket == null || !m_socket.Connected)
			{
				LogManager.Critical("소켓 연결 안 됨");
				return;
			}

			SocketAsyncEventArgs sendEventArgs = SocketAsyncEventArgsPool.Instance.Pop();
			if (sendEventArgs == null)
			{
				LogManager.Critical("SocketAsyncEventArgsPool::Pop() returns null");
				return;
			}

			sendEventArgs.Completed += onSendComplete;
			sendEventArgs.UserToken = this;
		}

		void onSendComplete(object sender, SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				LogManager.Success("전송 성공");
			}
			else
			{
				LogManager.Warn("전송 실패");
			}

			e.Completed -= onSendComplete;
			SocketAsyncEventArgsPool.Instance.Push(e);
		}

		#region 메시지 받기

		SocketAsyncEventArgs m_receiveEventArgs;
		MessageResolver m_messageResolver;
		LinkedList<Packet> m_receivePackList;
		GamePacketHandler m_gamePacketHandler;
		byte[] m_receiveBuffer;

		public void Init()
		{
			m_receivePackList = new LinkedList<Packet>();
			m_receiveBuffer = new byte[1024 * 4];
			m_messageResolver = new MessageResolver();
			
			m_gamePacketHandler = new GamePacketHandler();
			m_gamePacketHandler.Init(this);
			
			m_receiveEventArgs = new SocketAsyncEventArgs();
			m_receiveEventArgs.Completed += onReceiveCompleted;
			m_receiveEventArgs.UserToken = this;
			m_receiveEventArgs.SetBuffer(m_receiveBuffer, 0, 1024 * 4);
		}

		void StartReceive()
		{
			bool pending = m_socket.ReceiveAsync(m_receiveEventArgs);
			if (!pending)
			{
				onReceiveCompleted(this, m_receiveEventArgs);
			}
		}

		void onReceiveCompleted(object sender, SocketAsyncEventArgs e)
		{
			if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
			{
				m_messageResolver.OnReceive(e.Buffer, e.Offset, e.BytesTransferred, onMessageCompleted);
				StartReceive();
			}
			else
			{
				LogManager.Warn("메시지 수신 실패 - 서버 닫힘 or 통신 불가");
			}
		}

		void onMessageCompleted(Packet packet)
		{
			PushPacket(packet);
		}

		void PushPacket(Packet packet)
		{
			// todo mutex
			// lock (m_mutexReceivePackList)
			// {
			// 	
			m_receivePackList.AddLast(packet);
			// }
		}

		public void ProcessPackets()
		{
			// todo mutex
			// lock (m_mutexReceivePackList)
			// {
			// 	
			foreach (Packet packet in m_receivePackList)
			{
				m_gamePacketHandler.ParsePacket(packet);
			}
			
			m_receivePackList.Clear();
			// }
		}
		
		#endregion

	}
}