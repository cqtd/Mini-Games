using UnityEngine;

namespace CQ.MiniGames
{
	public class Bootstrap : MonoBehaviour
	{
		public int targetFramerate = 60;

		void Awake()
		{
			Application.targetFrameRate = targetFramerate;
		}
	}
}