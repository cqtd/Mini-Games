using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Sockets;
using Service.TCP;

namespace Service.TCP.Server
{
	public class UserToken
	{
		public User m_user;
		SocketAsyncEventArgs m_receiveEventArgs;
		MessageResolver messageResolver;
		Socket m_socket;

		public Socket Socket {
			get
			{
				return m_socket;
			}
			set
			{
				m_socket = value;
			}
		}

		List<Packet> m_packedList = new List<Packet>();
		object m_mutex_packedList = new object();

		public UserToken()
		{
			messageResolver = new MessageResolver();
		}

		public void Init()
		{
			m_receiveEventArgs = new SocketAsyncEventArgs();
			m_receiveEventArgs.Completed += onReceiveCompleted;
			m_receiveEventArgs.UserToken = this;

			BufferManager.Instance.SetBuffer(m_receiveEventArgs);
		}

		public void StartReceive()
		{
			bool pending = m_socket.ReceiveAsync(m_receiveEventArgs);
			if (!pending)
			{
				onReceiveCompleted(this, m_receiveEventArgs);
			}
		}

		public void Send(Packet packet)
		{
			SocketAsyncEventArgs sendEventArgs = SocketAsyncEventArgsPool.Instance.Pop();
			if (sendEventArgs == null)
			{
				LogManager.Critical("SocketAsyncEventArgsPool::Pop() returns null");
				return;
			}

			sendEventArgs.Completed += onSendCompleted;
			sendEventArgs.UserToken = this;

			byte[] sendData = packet.GetSendBytes();
			sendEventArgs.SetBuffer(sendData, 0, sendData.Length);

			bool pending = m_socket.SendAsync(sendEventArgs);
			if (!pending)
			{
				onSendCompleted(null, sendEventArgs);
			}
		}

		void onReceiveCompleted(object sender, SocketAsyncEventArgs e)
		{
			if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
			{
				messageResolver.OnReceive(e.Buffer, e.Offset, e.BytesTransferred, onMessageCompleted);
				StartReceive();
			}
			else
			{
				LogManager.Critical("잘못된 메시지 or 유저 연결 끊김");
			}
		}

		void onSendCompleted(object sender, SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				// 성공
			}
			else
			{
				// 실패
			}

			e.Completed -= onSendCompleted;
			SocketAsyncEventArgsPool.Instance.Push(e);
		}

		void onMessageCompleted(Packet packet)
		{
			AddPacket(packet);
		}

		public void AddPacket(Packet packet)
		{
			lock (m_mutex_packedList)
			{
				m_packedList.Add(packet);	
			}
		}


		public void Update()
		{
			if (m_packedList.Count > 0)
			{
				lock (m_mutex_packedList)
				{
					try
					{
						foreach (Packet packet in m_packedList)
						{
							m_user.ProcessPacket(packet);
						}

						m_packedList.Clear();
					}
					catch (Exception e)
					{
						LogManager.Critical(e.Message);
					}
				}
			}
		}

		public void Close()
		{
			try
			{
				if (m_socket != null)
				{
					m_socket.Shutdown(SocketShutdown.Both);
				}
			}
			catch (Exception e)
			{
				LogManager.Critical(e.Message);
			}
			finally
			{
				if (m_socket != null)
				{
					m_socket.Close();
				}
			}

			m_socket = null;
			m_user = null;
			messageResolver.ClearBuffer();

			BufferManager.Instance.FreeBuffer(m_receiveEventArgs);

			if (m_receiveEventArgs != null)
			{
				m_receiveEventArgs.Dispose();
			}

			m_receiveEventArgs = null;
		}
	}
}