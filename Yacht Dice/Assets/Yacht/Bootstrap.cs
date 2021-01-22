using System;
using System.Collections;
using UnityEngine;

namespace Yacht
{
	[DefaultExecutionOrder(0)]
	[AddComponentMenu("Yacht/Bootstrap")]
	public class Bootstrap : MonoBehaviour
	{
		public static event Action screen_manager; 
		public static event Action on_animation_load; 
		
		private IEnumerator Start()
		{
			Dispatcher.Init();
			Engine.Init();

			World.Init();
			// ScreenManager.Init();
			screen_manager?.Invoke();
			Game.Init();
			
			Game.Instance.CreateNewGame();

			Physics.queriesHitTriggers = true;
			
			yield return Patchable.Instance.CheckUpdates();
			
			Patchable.Instance.LoadAnimations(on_animation_load);
		}
	}
}