using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace CQ.MiniGames.Tests
{
	using Yacht.ReplaySystem;
	using Yacht;
	
	public class AnimationBakeTest
	{
		RecordedRollPack pack = default;
		DiceAnimatior animator = default;
		Transform[] diceRoots = default;
		
		const float unitTestTimeScale = 5.0f;
		
		/// <summary>
		/// 애니메이션 베이킹이 잘 되었는지 확인
		/// </summary>
		/// <returns></returns>
		[UnityTest]
		public IEnumerator AnimationBakeTestWithEnumeratorPasses()
		{
			yield return SceneManager.LoadSceneAsync("Dices");
			yield return new WaitForSeconds(1.0f);

			animator = Object.FindObjectOfType<DiceAnimatior>();
			diceRoots = animator.diceRoots.Select(e => e.transform).ToArray();
			pack = animator.pack;

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

			yield return null;
			yield return UnitTests(entries);
		}

		IEnumerator UnitTests(List<List<int>> cases)
		{
			Time.timeScale = unitTestTimeScale;
			yield return new WaitForSeconds(3);

			Object.FindObjectOfType<PhysicalSimulator>().gameObject.SetActive(false);

			var buttons = Object.FindObjectsOfType<Button>();
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
						datas = new List<RecordedRoll>();
						break;
				}

				foreach (RecordedRoll recorded in datas)
				{
					yield return animator.Playing(recorded, values);

					for (int i = 0; i < recorded.datas.Length; i++)
					{
						Enums.DiceFace upperFace = DiceResolver.GetResult(diceRoots[i].GetChild(0), 1.5f);

						if (upperFace == Enums.DiceFace.UNDEFINED)
						{
							Debug.LogWarning(
								$"Failed : {index}:{values.Count}:{recorded.name} {upperFace}!={recorded.datas[i].upside} {values[i]}");
							UnityEditor.EditorApplication.isPaused = true;

							continue;
						}

						Assert.IsTrue((int) upperFace == values[i]);

						if ((int) upperFace == values[i])
						{
							Debug.Log(
								$"Success : {index}:{values.Count}:{recorded.name} {upperFace}={recorded.datas[i].upside}, {values[i]}");
						}
						else
						{
							Debug.LogWarning(
								$"Failed : {index} : {i} : {recorded.name} {upperFace}!={recorded.datas[i].upside} {values[i]}");
						}
					}

					yield return new WaitForSeconds(0.5f);
				}
			}
		}
	}
}
