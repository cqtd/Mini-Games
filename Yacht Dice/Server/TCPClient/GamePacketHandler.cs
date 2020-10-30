using Service.TCP;

namespace Service.TCP.Client
{
	public class GamePacketHandler
	{
		Network m_network;

		public void Init(Network network)
		{
			this.m_network = network;
		}

		public void ParsePacket(Packet packet)
		{
			switch ((EPacketType)packet.m_type)
			{
				case EPacketType.TEST_PACKET_RES:
					TestPacketRes(packet);
					break;
			}
		}

		public void TestPacketRes(Packet packet)
		{
			
		}
	}
}