namespace Service.TCP.Server
{
	public class User
	{
		public UserToken UserToken;

		public void Init(UserToken token)
		{
			UserToken = token;
		}

		public void ProcessPacket(Packet packet)
		{
			
		}
	}
}