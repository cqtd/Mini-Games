namespace Service.TCP.Server
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var listener = new Listener();
			BufferManager.Instance.Init();
			
			listener.Start("0.0.0.0", 7070, 7072);
		}
	}
}