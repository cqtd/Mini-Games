using System;
using System.ComponentModel;

namespace Service.TCP
{
	public class Packet
	{
		public short m_type { get; set; }
		public byte[] m_data { get; set; }

		public Packet()
		{
			
		}

		public void SetData(byte[] data, int len)
		{
			m_data = new byte[len];
			Array.Copy(data, m_data, len);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public byte[] GetSendBytes()
		{
			byte[] typeBytes = BitConverter.GetBytes(m_type);
			int headerSize = (int) (m_data.Length);
			
			byte[] headerBytes = BitConverter.GetBytes(headerSize);
			byte[] sendBytes = new byte[headerBytes.Length + typeBytes.Length + m_data
				.Length];
			
			Array.Copy(headerBytes, 0, sendBytes, 0, headerBytes.Length);
			Array.Copy(typeBytes, 0, sendBytes, headerBytes.Length, typeBytes.Length);
			Array.Copy(m_data, 0, sendBytes, headerBytes.Length+typeBytes.Length , m_data.Length);

			return sendBytes;
		}
	}

	public enum EPacketType
	{
		NONE = -1,
		
		TEST_PACKET_RES,
		TEST_PACKET_REQ,
		
		PACKET_COUNT,
	}
}