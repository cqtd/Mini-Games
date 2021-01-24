using System;
using Yacht.Gameplay;

namespace Yacht
{
	public class Game : DynamicMonoSingleton<Game>
	{
		public Player Player { get; protected set; }

		public event Action onGameCreate;

		public void CreateNewGame()
		{
			Player = new Player();
			
			onGameCreate?.Invoke();
		}

		public override void Initialize()
		{
			
		}
	}
}