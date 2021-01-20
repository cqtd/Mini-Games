using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Yacht
{
	using AssetManagement;
	using ReplaySystem;
	
	/// <summary>
	/// This singleton provides static data which is patchable as streaming assets.
	/// </summary>
	public class Patchable : SingletonMono<Patchable>
	{
		public IEnumerator CheckUpdates()
		{
			yield return null;
		}

		public void LoadSprites() { }

		public Dictionary<int, List<RollingAnimation>> animationMap;

		public bool IsAnimationLoaded {
			get => isAnimationLoaded;
		}

		private bool isAnimationLoaded = false;
		private Action onComplete;

		private int count = 0;

		public event Action<float> onLoadingProgressUpdate;


		private void OnLoadingComplete(bool success)
		{
			isAnimationLoaded = true;

			onComplete?.Invoke();
			onComplete = null;
		}
		
		public void LoadAnimations(Action callback)
		{
			if (isAnimationLoaded) return;

			onComplete = callback;

			Debug.Log($"Loading animations has started. ");
			StartCoroutine(LoadAnimationCoroutine(OnLoadingComplete));
		}

		private IEnumerator LoadAnimationCoroutine(Action<bool> callback)
		{
			Engine.Log(Application.streamingAssetsPath);

			Dictionary<int, List<string>> deserialized;
			string dir;
			byte[] bytes;
			string json;

			animationMap = new Dictionary<int, List<RollingAnimation>>();

			dir = Application.streamingAssetsPath + Constant.PATCHABLE;

			UnityWebRequest req = UnityWebRequest.Get(dir + $"/hash.bin");
			yield return req.SendWebRequest();

			if (req.result != UnityWebRequest.Result.Success)
			{
				Engine.LogError(req.error);
				callback.Invoke(false);
				
				yield break;
			}

			bytes = req.downloadHandler.data;
			req.Dispose();

			json = GZipCompress.Unzip(bytes);
			json = GZipCompress.XORCipher(json, Constant.TEJAVA);

			deserialized = JsonConvert.DeserializeObject<Dictionary<int, List<string>>>(json);

			int total = deserialized.Keys.Sum(e => deserialized[e].Count);

			foreach (int key in deserialized.Keys)
			{
				var hashes = deserialized[key];
				foreach (string hash in hashes)
				{
					count++;

					string filename = $"{hash}.{Constant.DICE_ANIM_EXTENSION}";
					string path = dir + "/" + filename; 

					Engine.Log($"[{count:00}/{total:00}] :: {filename}");
					
					onLoadingProgressUpdate?.Invoke(100f * count / total);

					req = UnityWebRequest.Get(path);

					yield return req.SendWebRequest();

					if (req.result != UnityWebRequest.Result.Success)
					{
						Engine.LogError(req.error);
						callback.Invoke(false);

						yield break;
					}

					bytes = req.downloadHandler.data;
					req.Dispose();

					bool done = false;

					DecompressAsync(key, bytes, () => { done = true; });

					while (!done)
					{
						yield return null;
					}

					yield return null;
				}
			}

			onLoadingProgressUpdate?.Invoke(100.0f);
			Engine.Log("processing... done!");

			callback?.Invoke(true);

			async void DecompressAsync(int key, byte[] data, Action complete)
			{
				Dispatcher.BeginAsync();

				Task task = Task.Run(() =>
				{
					json = GZipCompress.Unzip(data);
					json = GZipCompress.XORCipher(json, Constant.TEJAVA);

					RollingAnimation animObj = JsonConvert.DeserializeObject<RollingAnimation>(json);

					if (!animationMap.TryGetValue(key, out var list))
					{
						list = new List<RollingAnimation>();
					}

					list.Add(animObj);
					animationMap[key] = list;
				});

				await task;

				Dispatcher.EndAsync();

				complete?.Invoke();
			}
		}
	}
}