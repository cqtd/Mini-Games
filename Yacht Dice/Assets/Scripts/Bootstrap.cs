using System.Collections;
using UnityEngine;

namespace CQ.MiniGames
{
	public class Bootstrap : MonoBehaviour
	{
		public ETargetFramerate targetFramerate = ETargetFramerate._60;

		private const string USER_INTERFACE_SCENE = "Scenes/UIScene";

		private void Awake()
		{
			Application.targetFrameRate = (int) targetFramerate;

			StartCoroutine(Initialize());
		}

		private IEnumerator Initialize()
		{
			yield return InitializeAssetManager();
			yield return InitializeAtlasManager();
			
			yield return LoadUserInterface();

			yield return LoadGooglePlayGameService();
		}

		private IEnumerator LoadUserInterface()
		{
			yield return null;
		}

		private IEnumerator InitializeAssetManager()
		{
			yield return null;
		}

		private IEnumerator LoadGooglePlayGameService()
		{
			yield return null;
		}

		private IEnumerator InitializeAtlasManager()
		{
			yield return null;
		}
	}

	public enum ETargetFramerate
	{
		_1 = 1,
		
		_15 = 15,
		_24 = 24,
		_30 = 30,
		
		_60 = 60,
		
		_75 = 75,
		
		_120 = 120,
		_180 = 180,
		_240 = 240,
		
		LIMITLESS = -1,
	}
}