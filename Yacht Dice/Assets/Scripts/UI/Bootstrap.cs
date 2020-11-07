using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames
{
	public class Bootstrap : MonoBehaviour
	{
		public int targetFramerate = 60;

		// public GameObject character;

		void Awake()
		{
			Application.targetFrameRate = targetFramerate;
			// var a = character.GetComponent<Image>();
			
		}
	}
}