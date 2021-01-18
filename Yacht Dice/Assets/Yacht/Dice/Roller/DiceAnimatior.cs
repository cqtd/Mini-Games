using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Yacht.ReplaySystem
{
	using AssetManagement;

	public class DiceAnimatior : MonoBehaviour
	{
		[SerializeField] public VisualDice[] diceRoots = default;

		private Dictionary<int, List<RollingAnimation>> animationMap;
		
		private bool isPaused = false;
		private bool isPlaying = false;

		private bool isAnimationLoaded = false;
		
		private Coroutine playCoroutine;
		private Action onComplete;

		private CancellationTokenSource tokenSource2;
		private CancellationToken ct;
		private Task loadingTask;

		private int elapsedSecond = 0;
		private int count = 0;

		
		private void Awake()
		{
			foreach (VisualDice diceRoot in diceRoots)
			{
				diceRoot.gameObject.SetActive(false);
			}
		}

		private void Start()
		{
			
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			if (loadingTask != null && !loadingTask.IsCompleted)
			{
				tokenSource2.Cancel(false);
			}
#endif
			
			LoadAnimationAsync();
		}
		
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		private void OnDisable()
		{
			tokenSource2?.Cancel(false);
			tokenSource2?.Dispose();
		}
#endif

		private async void LoadAnimationAsync()
		{
			Debug.Log($"Loading animations has started. ");
			
			tokenSource2 = new CancellationTokenSource();
			ct = tokenSource2.Token;
			
			loadingTask = Task.Run(WorkThreadAsync, ct);
			await loadingTask;
			loadingTask = null;

			isAnimationLoaded = true;
			
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Debug.Log($"{count} animations were loaded. ({elapsedSecond}s)");
#endif
		}

		private void WorkThreadAsync()
		{
			
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
#endif

			animationMap = new Dictionary<int, List<RollingAnimation>>();

			string dir = Application.streamingAssetsPath + Constant.PATCHABLE;

			byte[] bytes = File.ReadAllBytes(dir + $"/hash.bin");
			string json = GZipCompress.Unzip(bytes);
			json = GZipCompress.XORCipher(json, Constant.TEJAVA);

			var deserialized = JsonConvert.DeserializeObject<Dictionary<int, List<string>>>(json);

			foreach (int key in deserialized.Keys)
			{
				var hashes = deserialized[key];
				foreach (string hash in hashes)
				{

#if UNITY_EDITOR || DEVELOPMENT_BUILD

					if (ct.IsCancellationRequested)
					{
						Debug.Log("WorkThread::Aborted.");
						return;
					}
					
#endif

					count++;

					string path = dir + $"/{hash}.{Constant.DICE_ANIM_EXTENSION}";

					bytes = File.ReadAllBytes(path);
					json = GZipCompress.Unzip(bytes);
					json = GZipCompress.XORCipher(json, Constant.TEJAVA);

					RollingAnimation animObj = JsonConvert.DeserializeObject<RollingAnimation>(json);

					if (!animationMap.TryGetValue(key, out var list))
					{
						list = new List<RollingAnimation>();
					}

					list.Add(animObj);
					animationMap[key] = list;
				}
			}
			
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			elapsedSecond = stopwatch.Elapsed.Seconds;
#endif
			
		}

		public bool IsPlaying()
		{
			return isPlaying;
		}

		public void Play(List<int> dices, Action callback = null)
		{
			if (!isAnimationLoaded) return;

			onComplete = callback;
			
			var animationPack = animationMap[dices.Count];
			RollingAnimation recorded = animationPack[Random.Range(0, animationPack.Count - 1)];

			playCoroutine = StartCoroutine(Playing(recorded, dices));
		}

		public void Pause()
		{
			if (!isPlaying)
			{
				Debug.LogError($"애니메이션이 재생중이아닙니다.");
				return;
			}
			
			isPaused = true;
		}

		public void Stop(bool completeCallback)
		{
			if (isPlaying && playCoroutine != null)
			{
				StopCoroutine(playCoroutine);
				
				if (completeCallback)
				{
					onComplete?.Invoke();
				}
			}

			isPaused = false;
			isPlaying = false;
			onComplete = null;
		}

		private IEnumerator Playing(RollingAnimation rollAnim, List<int> dices)
		{
			isPlaying = true;
			
			// 다이스 비주얼라이즈 온 오프
			// 다이스 초기 세팅
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				if (i >= rollAnim.datas.Length)
				{
					diceRoots[i].gameObject.SetActive(false);
					diceRoots[i].Hide();
					continue;
				}
				
				diceRoots[i].gameObject.SetActive(true);
				diceRoots[i].Show();

				diceRoots[i].transform.GetChild(0).localRotation =
					Quaternion.Euler(rollAnim.datas[i].upside.GetLocalRotation((Enums.DiceValue) dices[i]));
			}
			
			// 재생 로직
			float elapsedTime = rollAnim.length;
			float t = 0.0f;
			
			while (true)
			{
				for (int i = 0; i < rollAnim.datas.Length; i++)
				{
					rollAnim.datas[i].Set(t, diceRoots[i].transform);
				}

				t += Time.deltaTime * Time.timeScale;
				
				// 재생 끝
				if (t > elapsedTime)
				{
					Debug.Log($"리플레이 종료");
					break;
				}

				// 일시정지
				while (isPaused)
				{
					yield return null;
				}

				yield return null;
			}
			
			// 콜백 호출
			onComplete?.Invoke();
			
			// 클린
			isPlaying = false;
			onComplete = null;
		}

#if UNITY_EDITOR
		
		[SerializeField] private List<int> diceValues = default;

		[ContextMenu("Play")]
		private void Play()
		{
			if (!Application.isPlaying) return;
			if (!isAnimationLoaded) return;
			
			Play(diceValues);
		}
		
		[ContextMenu("Play",true)]
		private bool CanEditorPlay()
		{
			if (!Application.isPlaying) return false;
			if (!isAnimationLoaded) return false;

			return true;
		}
#endif
	}
}