using System.Net.Sockets;
using CQ;

namespace Service.TCP.Server
{
	public class NetworkManager : Singleton<NetworkManager>
	{
		public void OnNewClient(Socket client_socket, object event_args)
		{
			UserToken token = new UserToken();
			token.Init();

			// User user = UserPool.Instance.Pop();
			User user = new User();
			user.Init(token);

			token.m_user = user;

			user.UserToken.Socket = client_socket;

			user.UserToken.Socket.NoDelay = true;
			user.UserToken.Socket.ReceiveTimeout = 60 * 1000;
			user.UserToken.Socket.SendTimeout = 60 * 1000;

			user.UserToken.StartReceive();

			UserManager.Instance.AddCommonRequestData();
		}
	}
}