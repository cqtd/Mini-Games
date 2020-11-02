using CQ.UI;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace CQ.MiniGames
{
	public class LobbyMenuCanvas : UICanvas
	{
		public Button gameStartButton = default;
		public CanvasGroup blurGroup = default;
		public float blurMax = 0.75f;
		public float blurDuration = 1.0f;

		public GameSelectWindow gameSelectWindow = default;

		public void Blurize()
		{
			var tweener = blurGroup.DOFade(blurMax, blurDuration);
			
			blurGroup.blocksRaycasts = true;
			blurGroup.interactable = true;
		}

		public void Unblurize()
		{
			var tweener = blurGroup.DOFade(0, blurDuration);
			tweener.OnComplete(() =>
			{
				blurGroup.blocksRaycasts = false;
				blurGroup.interactable = false;
			});
		}
		
		protected override void InitComponent()
		{
			blurGroup.alpha = 0;
			gameSelectWindow.InitComponent();
			
			gameStartButton.onClick.AddListener(OnGameSelectButton);
			blurGroup.GetComponent<Button>().onClick.AddListener(OnCancel);
		}

		public override void Dispose()
		{
			
		}

		public void OnGameSelectButton()
		{
			Blurize();
			gameSelectWindow.Open();
		}

		public void OnCancel()
		{
			Unblurize();
			gameSelectWindow.Close();
		}
	}
}