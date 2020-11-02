using CQ.UI;

namespace CQ.MiniGames
{
	public class GameSelectWindow : UIWindow
	{
		public GameModeBox left = default;
		public GameModeBox right = default;
		
		
		public void InitComponent()
		{
			left.InitComponent();
			right.InitComponent();
		}

		public void Open()
		{
			left.Show();
			right.Show();
		}

		public void Close()
		{
			left.Hide();
			right.Hide();
		}
	}
}