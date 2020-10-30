namespace Service.TCP.Client
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var network = new Network();
			network.Init();
			network.Connect("127.0.0.1", 7070);
		}
	}
}