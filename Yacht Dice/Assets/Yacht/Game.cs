using System;
using UnityEngine;
using Yacht.Gameplay;

namespace Yacht
{
	public class Game : MonoBehaviour
	{
		public static Game Instance {
			get => instance;
		}
		private static Game instance;
		
		public static void Init()
		{
			if (instance == null)
			{
				instance = FindObjectOfType<Game>();
				if (instance == null)
				{
					instance = new GameObject("[Game]").AddComponent<Game>();
				}
			}
		}

		public Player Player { get; protected set; }

		public event Action onGameCreate;

		public void CreateNewGame()
		{
			Player = new Player();
			
			onGameCreate?.Invoke();
		}
		
		public enum GameState
		{
			NONE,
			
			BEFORE_ROUND,
			ANIMATING,
			WAIT_FOR_PLAYER,
			
			AFTER_GAME,
		}
	}
}