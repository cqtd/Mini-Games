using System;
using System.Collections;
using System.Collections.Generic;
using CQ.MiniGames;
using DG.Tweening;
using MEC;
using UnityEngine;
using Yacht.Gameplay;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

namespace Yacht.ReplaySystem
{
	using AssetManagement;

	public class DiceAnimatior : MonoBehaviour
	{
		[SerializeField] public VisualDice[] diceRoots = default;

		private bool isPaused = false;
		private bool isPlaying = false;

		private Coroutine playCoroutine;
		private Action onComplete;
		
		private void Awake()
		{
			foreach (VisualDice diceRoot in diceRoots)
			{
				diceRoot.gameObject.SetActive(false);
			}
		}
		

		public List<int> GetUnlockedIndecies()
		{
			var entry = new List<int>();
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				if (!diceRoots[i].IsLocked)
				{
					entry.Add(i);
				}
			}

			return entry;
		}

		private void PlayRandom()
		{
			var dices = new List<int>();

			int diceCount = 5;
			// int diceCount = Random.Range(1, 5);
			for (int i = 0; i < diceCount; i++)
			{
				dices.Add(Random.Range(1, 6));
			}
				
			Play(dices, () =>
			{
				int score = Scoresheet.GetBest(dices, out var bestFit);
				Engine.Log($"{bestFit}! - {score}");

				ViewDice();
			});
		}

		public bool IsPlaying()
		{
			return isPlaying;
		}

		public void Play(List<int> dices, Action callback = null)
		{
			if (!Patchable.Instance.IsAnimationLoaded) return;
			if (isPlaying)
			{

				UnityEngine.Debug.Log("재생 중입니다. 애니메이션 종료를 기다려주세요.");

				return;
			}

			onComplete = callback;
			
			var animationPack = Patchable.Instance.animationMap[dices.Count];
			RollingAnimation recorded = animationPack[Random.Range(0, animationPack.Count - 1)];

			playCoroutine = StartCoroutine(Playing(recorded, dices));
		}

		public void Pause()
		{
			if (!isPlaying)
			{
				UnityEngine.Debug.LogError($"애니메이션이 재생중이아닙니다.");
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

			List<int> wildcards = new List<int>();
			
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

				diceRoots[i].DiceValue = dices[i];
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
			
			ViewDice();
		}

		private void ViewDice()
		{
			for (int i = 0; i < diceRoots.Length; i++)
			{
				int index = i;
				
				DiceBase dice = diceRoots[index];
				dice.CacheTransform();

				Timing.CallDelayed(i * 0.1f, () =>
				{
					Transform prevParent = dice.transform.parent;
					
					dice.transform.SetParent(World.ViewPosition[index]);
					dice.transform.DOLocalMove(Vector3.zero, 0.4f);

					Vector3 rotation = Vector3.zero;
					switch (dice.GetComponent<DiceBase>().DiceValue)
					{
						case 1:
							rotation = Vector3.up * 180f;
							break;
						case 2:
							rotation = Vector3.right * -90f;
							break;
						case 3:
							rotation = Vector3.up * 90f;
							break;
						case 4:
							rotation = Vector3.up * -90f;
							break;
						case 5:
							rotation = Vector3.right * 90f;
							break;
						case 6:
							break;
						default:
							break;
					}

					Tweener rotater = dice.transform.DOLocalRotate(rotation, 0.4f);
					rotater.OnComplete(() =>
					{
						dice.transform.SetParent(prevParent);
					});
				});
			}
		}
	}
}