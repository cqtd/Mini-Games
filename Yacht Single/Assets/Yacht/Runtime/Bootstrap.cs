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
			Dispatcher.Instance.Initialize();
			Engine.Instance.Initialize();
			
			World.Setup();
			
			screen_manager?.Invoke();
			
			Game.Instance.Initialize();
			Game.Instance.CreateNewGame();

			Physics.queriesHitTriggers = true;
			
			yield return Patchable.Instance.CheckUpdates();
			
			Patchable.Instance.LoadAnimations(on_animation_load);
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetDomain()
		{
			screen_manager = null;
			on_animation_load = null;
		}
	}
}