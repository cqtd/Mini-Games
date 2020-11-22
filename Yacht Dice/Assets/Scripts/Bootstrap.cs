using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CQ.MiniGames
{
	public class Bootstrap : MonoBehaviour
	{
		public int targetFramerate = 60;

		const string USER_INTERFACE_SCENE = "Scenes/UIScene";

		void Awake()
		{
			Application.targetFrameRate = targetFramerate;

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
			// if (SceneManager.GetSceneByName(USER_INTERFACE_SCENE).isLoaded)
			// {
			// 	Debug.LogWarning($"Already [{USER_INTERFACE_SCENE}] Loaded!");
			// 	yield break;
			// }
			//
			// AsyncOperation operation = SceneManager.LoadSceneAsync(USER_INTERFACE_SCENE, LoadSceneMode.Additive);
			// yield return operation;
			//
			// Debug.Log($"Successfully [{USER_INTERFACE_SCENE}] Loaded!");
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
}