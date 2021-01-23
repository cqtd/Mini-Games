using System;
using UnityEngine;

namespace Yacht.UIToolkit
{
	public class CanvasManager : MonoBehaviour
	{
		public TitleCanvas title;
		public GameCanvas game;

		public static CanvasManager Instance { get; protected set; }

		private void Awake()
		{
			Instance = this;
		}

		public void ShowTitle()
		{
			title.gameObject.SetActive(true);
			game.gameObject.SetActive(false);
		}

		public void ShowGameView()
		{
			title.gameObject.SetActive(false);
			game.gameObject.SetActive(true);

			game.dicer.Initialize();
			
			game.Initialize();
		}
	}
}