using CQ.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace CQ.MiniGames
{
	public class YachtUIManager : UIManager
	{
		public Canvas splashMenu;
		[FormerlySerializedAs("lobbyMenuCanvas")] public LobbyCanvas lobbyCanvas;

		protected override void Awake()
		{
			base.Awake();
			
			lobbyCanvas.gameObject.SetActive(false);
		}

		public void OpenGameMenu()
		{
			lobbyCanvas.canvasGroup.alpha = 0;
			lobbyCanvas.gameObject.SetActive(true);
			Tweener tweener =lobbyCanvas.canvasGroup.DOFade(1.0f, Duration.Fast);
			tweener.OnComplete(() =>
			{
				splashMenu.gameObject.SetActive(false);
			});
			//
			// lobbyCanvas.gameObject.SetActive(true);
			// splashMenu.gameObject.SetActive(false);
		}
		
		/* 
		 * 
		 * Splash Canvas
		 * Lobby Canvas
		 * - Bottom Menu
		 * - Top Menu
		 * - GameStart Menu
		 * - Slide Menu
		 */ 
	}
}