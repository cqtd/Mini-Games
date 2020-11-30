using System.Collections;
using CQ.UI;
using UnityEngine.SceneManagement;

namespace CQ.MiniGames.UI
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
			// SceneManager.LoadScene("Dices");
			// YachtUIManager.Instance.gameObject.SetActive(false);

			StartCoroutine(LoadGameScene());
		}

		private void FadeIn()
		{
			
		}

		private IEnumerator FadeOut()
		{
			yield return null;
		}

		private IEnumerator LoadGameScene()
		{
			var operation = SceneManager.LoadSceneAsync("Scenes/Dices");
			operation.allowSceneActivation = false;
			
			yield return FadeOut();
			operation.allowSceneActivation = true;
			
			yield return operation;
			
			YachtUIManager.Instance.Close<LobbyCanvas>();
			YachtUIManager.Instance.Open<DiceTableCanvas>();
			
			FadeIn();
		}
	}
}