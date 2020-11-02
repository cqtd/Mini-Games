using CQ.UI;
using UnityEngine;

namespace CQ.MiniGames
{
	public class YachtUIManager : UIManager
	{
		public Canvas splashMenu;
		public LobbyMenuCanvas lobbyMenuCanvas;

		protected override void Awake()
		{
			base.Awake();
			
			lobbyMenuCanvas.gameObject.SetActive(false);
		}

		public void OpenGameMenu()
		{
			lobbyMenuCanvas.gameObject.SetActive(true);
			splashMenu.gameObject.SetActive(false);
		}
	}
}