using System;
using System.Collections;
using System.Collections.Generic;
using CQ.MiniGames.Yacht.ReplaySystem;
using CQ.MiniGames.Yacht;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CQ.MiniGames
{
	public class DiceAnimatior : MonoBehaviour
	{
		[SerializeField] public RecordedRollPack pack = default;
		[SerializeField] public Transform[] diceRoots = default;
		[SerializeField] List<int> diceValues = default;
		
		public float unitTestTimeScale = 5.0f;
		public bool pause = true;

		void Awake()
		{
			foreach (Transform diceRoot in diceRoots)
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
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				if (i >= recorded.datas.Length)
				{
					diceRoots[i].gameObject.SetActive(false);
					continue;
				}
				
				diceRoots[i].gameObject.SetActive(true);

				diceRoots[i].GetChild(0).localRotation =
					Quaternion.Euler(recorded.datas[i].upside.GetLocalRotation((Enums.DiceValue) dices[i]));
					
			}
			
			float elapsedTime = recorded.length;
			float t = 0.0f;
			while (true)
			{
				for (int i = 0; i < recorded.datas.Length; i++)
				{
					recorded.datas[i].Set(t, diceRoots[i]);
				}

				t += Time.deltaTime * Time.timeScale;
				
				if (t > elapsedTime)
				{
					Debug.Log($"리플레이 종료");
					break;
				}

				yield return null;
			}
			
			callback?.Invoke();
		}

		[ContextMenu("Play")]
		void Play()
		{
			Play(diceValues);
		}
	}
}