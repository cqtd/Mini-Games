using System;
using Service.TCP;

namespace Service.TCP
{
	public class MessageResolver
	{
		public delegate void CompletedMessageCallback(Packet packet);

		int m_messageSize;
		
		// 2000K
		byte[] m_messageBuffer = new byte[1024 * 2000]; 
		// 4 byte
		byte[] m_headerBuffer = new byte[4];
		// 2 byte
		byte[] m_typeBuffer = new byte[2];

		EPacketType m_preType;

		int m_headPosition;
		int m_typePosition;
		int m_currentPosition;

		short m_messageType;
		int m_remainBytes;

		bool m_headCompleted;
		bool m_typeCompleted;
		bool m_completed;

		CompletedMessageCallback m_completeCallback;

		public MessageResolver()
		{
			ClearBuffer();
		}

		public void OnReceive(byte[] buffer, int offset, int transffered, CompletedMessageCallback callback)
		{
			int src_position = offset;

			m_completeCallback = callback;
			m_remainBytes = transffered;

			if (!m_headCompleted)
			{
				m_headCompleted = ReadHead(buffer, ref src_position);

				if (!m_headCompleted)
				{
					return;
				}

				m_messageSize = GetBodySize();

				if (m_messageSize < 0 ||
				    m_messageSize > CommonDefine.COMPLETE_MESSAGE_SIZE_CLIENT)
				{
					return;
				}
			}

			if (!m_typeCompleted)
			{
				m_typeCompleted = ReadType(buffer, ref src_position);

				if (!m_typeCompleted)
				{
					return;
				}

				m_messageType = BitConverter.ToInt16(m_typeBuffer, 0);

				if (m_messageType < 0 ||
				    m_messageType > (int) EPacketType.PACKET_COUNT)
				{
					return;
				}

				m_preType = (EPacketType) m_messageType;
			}

			if (!m_completed)
			{
				m_completed = ReadBody(buffer, ref src_position);
				if (!m_completed)
				{
					return;
				}
			}
			
			Packet packet = new Packet();
			packet.m_type = m_messageType;
			packet.SetData(m_messageBuffer, m_messageSize);

			m_completeCallback(packet);
			ClearBuffer();
		}

		public void ClearBuffer()
		{
			Array.Clear(m_messageBuffer, 0, m_messageBuffer.Length);
			Array.Clear(m_headerBuffer, 0, m_headerBuffer.Length);
			Array.Clear(m_typeBuffer, 0, m_typeBuffer.Length);

			m_messageSize = 0;
			m_headPosition = 0;
			m_typePosition = 0;
			m_currentPosition = 0;
			m_messageType = 0;

			m_headCompleted = false;
			m_typeCompleted = false;
			m_completed = false;
		}

		bool ReadHead(byte[] buffer, ref int src_position)
		{
			return ReadUntil_Internal(buffer, ref src_position, m_headerBuffer, ref m_headPosition, 4);
		}
		
		bool ReadType(byte[] buffer, ref int src_position)
		{
			return ReadUntil_Internal(buffer, ref src_position, m_typeBuffer, ref m_typePosition, 2);
		}
		
		bool ReadBody(byte[] buffer, ref int src_position)
		{
			return ReadUntil_Internal(buffer, ref src_position, m_messageBuffer, ref m_currentPosition, m_messageSize);
		}

		bool ReadUntil_Internal(byte[] buffer, ref int src_position, byte[] dest_buffer, ref int dest_position, int to_size)
		{
			if (m_remainBytes < 0)
			{
				return false;
			}

			int copySize = to_size - dest_position;
			if (m_remainBytes < copySize)
			{
				copySize = m_remainBytes;
			}
			
			Array.Copy(buffer, src_position, dest_buffer, dest_position, copySize);

			src_position += copySize;
			dest_position += copySize;
			m_remainBytes -= copySize;

			return !(dest_position < to_size);
		}

		int GetBodySize()
		{
			Type type = CommonDefine.HEADER_SIZE.GetType();
			if (type == typeof(Int16))
			{
				return BitConverter.ToInt16(m_headerBuffer, 0);
			}

			return BitConverter.ToInt32(m_headerBuffer, 0);
		}
	}
}