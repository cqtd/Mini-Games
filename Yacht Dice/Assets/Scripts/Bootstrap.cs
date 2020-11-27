using System.Collections;
using UnityEngine;

namespace CQ.MiniGames
{
	public class Bootstrap : MonoBehaviour
	{
		public ETargetFramerate targetFramerate = ETargetFramerate._60;

		const string USER_INTERFACE_SCENE = "Scenes/UIScene";

		void Awake()
		{
			Application.targetFrameRate = (int) targetFramerate;

			StartCoroutine(Initialize());
		}

		IEnumerator Initialize()
		{
			yield return InitializeAssetManager();
			yield return InitializeAtlasManager();
			
			yield return LoadUserInterface();

			yield return LoadGooglePlayGameService();
		}

		IEnumerator LoadUserInterface()
		{
			yield return null;
		}

		IEnumerator InitializeAssetManager()
		{
			yield return null;
		}

		IEnumerator LoadGooglePlayGameService()
		{
			yield return null;
		}

		IEnumerator InitializeAtlasManager()
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