using System.Collections;
using CQ.MiniGames;
using UnityEngine;

namespace Yacht
{
	using UIToolkit;
	
	[DefaultExecutionOrder(0)]
	[AddComponentMenu("Yacht/Bootstrap")]
	public class Bootstrap : MonoBehaviour
	{
		private IEnumerator Start()
		{
			Dispatcher.Init();
			Engine.Init();

			World.Init();
			ScreenManager.Init();
			Game.Init();
			
			yield return Patchable.Instance.CheckUpdates();
			
			Patchable.Instance.LoadAnimations(OnAnimationLoad);
		}

		private void OnAnimationLoad()
		{
			Engine.Log("OnAnimationLoad");
			
			FindObjectOfType<LoadingScreen>().FadeIn();
		}
	}
}