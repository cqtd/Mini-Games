using UnityEngine;

namespace CQ.MiniGames
{
	[DefaultExecutionOrder(ScriptOrder.Firstpass.Game)]
	public class Game : MonoBehaviour
	{
		public static Game Instance { get; protected set; }

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			else
			{
				Destroy(this.gameObject);
			}
		}
	}
}