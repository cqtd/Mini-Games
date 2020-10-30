using System.Collections.Generic;
using System.Net.Sockets;
using CQ;

namespace Service.TCP
{
	public class BufferManager : Singleton<BufferManager>
	{
		int m_numBytes;
		byte[] m_buffer;
		Stack<int> m_freeIndexPool;
		int m_currentIndex;
		int m_bufferSize;

		public BufferManager()
		{
			
		}

		public void Init()
		{
			m_numBytes = CommonDefine.MAX_CONNECTION * CommonDefine.SOCKET_BUFFER_SIZE * 2;
			m_currentIndex = 0;

			m_bufferSize = CommonDefine.SOCKET_BUFFER_SIZE;
			m_freeIndexPool = new Stack<int>();
			m_buffer = new byte[m_numBytes];
		}

		public bool SetBuffer(SocketAsyncEventArgs args)
		{
			if (m_freeIndexPool.Count > 0)
			{
				args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
			}
			else
			{
				if (m_numBytes < (m_currentIndex + m_bufferSize))
				{
					return false;
				}

				args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
				m_currentIndex += m_bufferSize;
			}

			return true;
		}

		public void FreeBuffer(SocketAsyncEventArgs args)
		{
			if (args == null)
			{
				return;
			}
			
			m_freeIndexPool.Push(args.Offset);
			args.SetBuffer(null, 0, 0);
		}
	}
}