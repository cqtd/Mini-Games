using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CQ.MiniGames
{
	using Yacht.Gameplay;
	using Yacht.Gameplay.ReplaySystem;
	
	public class DiceAnimatior : MonoBehaviour
	{
		[SerializeField] public RecordedRollPack pack = default;
		[SerializeField] public VisualDice[] diceRoots = default;
		[SerializeField] private List<int> diceValues = default;
		
		public float unitTestTimeScale = 5.0f;
		public bool pause = true;

		private void Awake()
		{
			foreach (var diceRoot in diceRoots)
			{
				diceRoot.gameObject.SetActive(false);
			}
		}

		public void Play(List<int> dices)
		{
			RecordedRoll recorded = null;
			
			switch (dices.Count)
			{
				case 1:
				{
					recorded = pack.dice1[Random.Range(0, pack.dice1.Count - 1)];
					break;
				}
				
				case 2:
				{
					recorded = pack.dice2[Random.Range(0, pack.dice2.Count - 1)];
					break;
				}
				
				case 3:
				{
					recorded = pack.dice3[Random.Range(0, pack.dice3.Count - 1)];
					break;
				}
				
				case 4:
				{
					recorded = pack.dice4[Random.Range(0, pack.dice4.Count - 1)];
					break;
				}
				
				case 5:
				{
					recorded = pack.dice5[Random.Range(0, pack.dice5.Count - 1)];
					break;
				}
				
				default:
					break;
			}

			StartCoroutine(Playing(recorded, dices));
		}

		public IEnumerator Playing(RecordedRoll recorded, List<int> dices, Action callback = null)
		{
			// 다이스 비주얼라이즈 온 오프
			// 다이스 초기 세팅
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				if (i >= recorded.datas.Length)
				{
					diceRoots[i].gameObject.SetActive(false);
					diceRoots[i].Hide();
					continue;
				}
				
				diceRoots[i].gameObject.SetActive(true);
				diceRoots[i].Show();

				diceRoots[i].transform.GetChild(0).localRotation =
					Quaternion.Euler(recorded.datas[i].upside.GetLocalRotation((Enums.DiceValue) dices[i]));
					
			}
			
			// 재생 로직
			float elapsedTime = recorded.length;
			float t = 0.0f;
			while (true)
			{
				for (int i = 0; i < recorded.datas.Length; i++)
				{
					recorded.datas[i].Set(t, diceRoots[i].transform);
				}

				t += Time.deltaTime * Time.timeScale;
				
				// 재생 끝
				if (t > elapsedTime)
				{
					Debug.Log($"리플레이 종료");
					break;
				}

				yield return null;
			}
			
			// 콜백 호출
			callback?.Invoke();
		}

		[ContextMenu("Play")]
		private void Play()
		{
			Play(diceValues);
		}
	}
}