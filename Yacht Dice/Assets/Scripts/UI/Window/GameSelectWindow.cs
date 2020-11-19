using CQ.UI;
using UnityEngine.SceneManagement;

namespace CQ.MiniGames
{
	public class GameSelectWindow : UIWindow
	{
		public GameModeBox left = default;
		public GameModeBox right = default;
		
		
		public override void InitComponent()
		{
			left.InitComponent();
			right.InitComponent();
		}

		public override void Open()
		{
			left.Show();
			right.Show();
		}

		public override void Close()
		{
			left.Hide();
			right.Hide();
		}

		public void OpenGameScene()
		{
			SceneManager.LoadScene("Dices");
			YachtUIManager.Instance.gameObject.SetActive(false);
		}
	}
}