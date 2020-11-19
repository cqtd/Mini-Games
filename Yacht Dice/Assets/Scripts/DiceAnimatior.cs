using System;
using System.Collections;
using System.Collections.Generic;
using CQ.MiniGames.ReplaySystem;
using CQ.MiniGames.Yacht;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CQ.MiniGames
{
	public class DiceAnimatior : MonoBehaviour
	{
		[SerializeField] RecordedRollPack pack = default;
		[SerializeField] Transform[] diceRoots = default;
		[SerializeField] List<int> diceValues = default;
		
		public float unitTestTimeScale = 5.0f;
		public bool pause = true;

		void Start()
		{
			List<List<int>> entries = new List<List<int>>();

			for (int i = 1; i <= 5; i++)
			{
				for (int j = 1; j <= 5; j++)
				{
					var list = new List<int>();
					for (int k = 0; k < j; k++)
					{
						list.Add(i);	
					}
					
					entries.Add(list);
				}
			}

			StartCoroutine(UnitTests(entries));
		}

		IEnumerator UnitTests(List<List<int>> cases)
		{
			Time.timeScale = unitTestTimeScale;
			yield return new WaitForSeconds(3);
			
			FindObjectOfType<DiceRollTest>().gameObject.SetActive(false);

			var buttons = FindObjectsOfType<Button>();
			foreach (Button button in buttons)
			{
				button.interactable = false;
			}

			for (int index = 0; index < cases.Count; index++)
			{
				List<int> values = cases[index];
				List<RecordedRoll> datas;
				switch (values.Count)
				{
					case 1:
						datas = pack.dice1;
						break;

					case 2:
						datas = pack.dice2;
						break;

					case 3:
						datas = pack.dice3;
						break;

					case 4:
						datas = pack.dice4;
						break;

					case 5:
						datas = pack.dice5;
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}

				foreach (RecordedRoll recorded in datas)
				{
					yield return Playing(recorded, values);

					for (int i = 0; i < recorded.datas.Length; i++)
					{
						Enums.DiceFace upperFace = DiceResolver.GetResult(diceRoots[i].GetChild(0), 1.5f);

						if (upperFace == Enums.DiceFace.UNDEFINED)
						{
							Debug.LogWarning($"Failed : {index}:{values.Count}:{recorded.name} {upperFace}!={recorded.datas[i].upside} {values[i]}");
							UnityEditor.EditorApplication.isPaused = true;

							continue;
						}

						if ((int)upperFace == values[i])
						{
							Debug.Log($"Success : {index}:{values.Count}:{recorded.name} {upperFace}={recorded.datas[i].upside}, {values[i]}");
						}
						else
						{
							Debug.LogWarning($"Failed : {index} : {i} : {recorded.name} {upperFace}!={recorded.datas[i].upside} {values[i]}");
							if (pause)
							{
								UnityEditor.Selection.objects = new[] {diceRoots[i].gameObject};
								UnityEditor.EditorApplication.isPaused = true;
							}
						}
					}

					yield return new WaitForSeconds(0.5f);
				}
			}

			UnityEditor.EditorApplication.isPlaying = false;
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

		IEnumerator Playing(RecordedRoll recorded, List<int> dices, Action callback = null)
		{
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				if (i >= recorded.datas.Length)
				{
					diceRoots[i].gameObject.SetActive(false);
					continue;
				}
				
				diceRoots[i].gameObject.SetActive(true);
				
				// Top 기준
				if (dices[i] == 1)
				{
					diceRoots[i].GetChild(0).localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
				}
				else if (dices[i] == 2)
				{
					diceRoots[i].GetChild(0).localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
				}
				else if (dices[i] == 3)
				{
					diceRoots[i].GetChild(0).localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
				}
				else if (dices[i] == 4)
				{
					diceRoots[i].GetChild(0).localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
				}
				else if (dices[i] == 5)
				{
					diceRoots[i].GetChild(0).localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
				}
				else if (dices[i] == 6)
				{
					diceRoots[i].GetChild(0).localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
				
				switch (recorded.datas[i].upside)
				{
					case Enums.DiceFace.FORWARD:
						diceRoots[i].GetChild(0).localRotation *= Quaternion.Euler(new Vector3(90, 0, 0));
						break;
					case Enums.DiceFace.BACKWARD:
						diceRoots[i].GetChild(0).localRotation *= Quaternion.Euler(new Vector3(-90, 0, 0));
						break;
					case Enums.DiceFace.RIGHT:
						diceRoots[i].GetChild(0).localRotation *= Quaternion.Euler(new Vector3(0, 0, -90));
						break;
					case Enums.DiceFace.LEFT:
						diceRoots[i].GetChild(0).localRotation *= Quaternion.Euler(new Vector3(0, 0, 90));
						break;
					case Enums.DiceFace.TOP:
						diceRoots[i].GetChild(0).localRotation *= Quaternion.Euler(new Vector3(0, 0, 0));
						break;
					case Enums.DiceFace.BOTTOM:
						diceRoots[i].GetChild(0).localRotation *= Quaternion.Euler(new Vector3(180, 0, 0));

						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				diceRoots[i].GetChild(0).localRotation =
					Quaternion.Euler(recorded.datas[i].upside.GetLocalRotation((Enums.DiceValue) dices[i]));
					
				// Debug.Log($"{recorded.datas[i].upside} : {(Enums.DiceValue) dices[i]}", diceRoots[i].gameObject);
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