using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CQ.MiniGames
{
	using Yacht.ReplaySystem;
	
	public class PhysicalSimulator : MonoBehaviour
	{
		public Transform startPosMarker = default;
		
		public Transform[] viewPosition;
		public Transform[] lockPosition;

		[Header("Random Parameter")]
		[Range(-10,10)] public float minOffset = -1;
		[Range(-10,10)] public float maxOffset = 1;

		[Space]
		[Range(-10,20)] public float minForce = 2;
		[Range(-10,20)] public float maxForce = 10;
		
		[Space]
		[Range(-360, 360)] public float minAngular = 0;
		[Range(-360, 360)] public float maxAnguler = 360;

		private PhysicsDice[] dices;

		public string pathFormat = "Assets/Animations/Dice {0}/recorded_{1}.asset";

		public bool createDices = false;
		public bool rollDices = false;
		
		public bool physicsDiceCreated { get; set; }

		private float recordStartTime;
		private float recordEndTime;

		[Range(1, 20)] public int animationCount = 10;

		private void Awake()
		{
			if (createDices)
			{
				CreatePhysicsDice();

				if (rollDices)
				{
					RollDice();
				}
			}
		}

		private void CreatePhysicsDice()
		{
			dices = new PhysicsDice[5];
			for (int i = 0; i < 5; i++)
			{
				dices[i] = Resource.Instantiate<PhysicsDice>(Paths.PHYSICS_DICE, transform);
				// dices[i] = Instantiate(Resources.Load<PhysicsDice>(Paths.PHYSICS_DICE), transform);
				dices[i].onLockStateChanged += OnLockStateChanged;
				dices[i].name = $"(Instance) Physics Dice {i:00}";
			}

			physicsDiceCreated = true;
		}

		private Vector3 GetRandomOffset(float min, float max)
		{
			return new Vector3(Random.Range(min, max), Random.Range(min, max),Random.Range(min, max));
		}

		public void CreateAnimations()
		{
			StartCoroutine(CreatingAnimation());
		}

		private IEnumerator CreatingAnimation()
		{
			yield return new WaitForSeconds(1f);
			
#if UNITY_EDITOR
			var pack = AssetDatabase.LoadAssetAtPath<RecordedRollPack>("Assets/Animations/RecordedRollPack.asset");
			for (int i = 1; i <= 5; i++)
			{
				var list = new List<RecordedRoll>();
				
				yield return null;

				for (int j = 0; j < i; j++)
				{
					dices[j].gameObject.SetActive(true);
				}
				
				for (int j = i; j < 5; j++)
				{
					dices[j].gameObject.SetActive(false);
				}

				for (int count = 0; count < animationCount; count++)
				{
					recordStartTime = Time.time;

					int complete = 0;

					for (int diceIndex = 0; diceIndex < i; diceIndex++)
					{
						
						Vector3 velocity = startPosMarker.forward * Random.Range(minForce, maxForce);
						Vector3 angular = GetRandomOffset(minAngular, maxAnguler);
						Vector3 position = startPosMarker.position + GetRandomOffset(minOffset, maxOffset);
						
						dices[diceIndex].SetCollidable(true);
						dices[diceIndex].SetSimulatable(true);
						dices[diceIndex].SetPosition(position);
						dices[diceIndex].SetVelocity(velocity, angular);
						
						dices[diceIndex].GetReplayEntity().Record(success =>
						{
							if (success)
							{
								
							}

							complete++;
						});
					}

					while (complete < i)
					{
						yield return null;
					}
					
					recordEndTime = Time.time;

					var recored = ScriptableObject.CreateInstance<RecordedRoll>();
					recored.datas = new RecordData[i];
					recored.length = recordEndTime - recordStartTime;

					for (int j = 0; j < i; j++)
					{
						recored.datas[j] = dices[j].GetReplayEntity().data;
					}
					
					AssetDatabase.CreateAsset(recored, string.Format(pathFormat, i, count));
					list.Add(recored);
					
					yield return new WaitForSeconds(1f);
				}

				switch (i)
				{
					case 1:
						pack.dice1 = list;
						break;
					case 2:
						pack.dice2 = list;
						break;
					case 3:
						pack.dice3 = list;
						break;
					case 4:
						pack.dice4 = list;
						break;
					case 5:
						pack.dice5 = list;
						break;
				}
				
				yield return new WaitForSeconds(1f);
			}
			
			EditorUtility.SetDirty(pack);
			AssetDatabase.SaveAssets();

			EditorApplication.isPlaying = false;
#endif
		}

		public void RollDice()
		{
			if (!physicsDiceCreated)
			{
				CreatePhysicsDice();
			}
			
			int doneCount = 0;
			void OnMovementStop()
			{
				doneCount++;
				
				// on every dice stopped
				if (doneCount == dices.Length)
				{
					recordEndTime = Time.time + 1.0f;
				}
			}
			
			foreach (PhysicsDice dice in dices)
			{
				if (dice.IsLocked)
				{
					continue;
				}
				
				Vector3 velocity = startPosMarker.forward * Random.Range(minForce, maxForce);
				Vector3 angular = GetRandomOffset(minAngular, maxAnguler);
				Vector3 position = startPosMarker.position + GetRandomOffset(minOffset, maxOffset);

				dice.SetCollidable(false);
				Tweener tweener = dice.transform.DOMove(position, 0.4f);

				dice.onMovementStop += OnMovementStop;
				
				tweener.OnComplete(() =>
				{
					recordStartTime = Time.time;

					dice.SetCollidable(true);
					dice.SetSimulatable(true);
					dice.SetVelocity(velocity, angular);
					
					dice.GetReplayEntity().Record();
				});
			}
		}

		public void ViewDice()
		{
			for (int i = 0; i < dices.Length; i++)
			{
				int index = i;
				Timing.CallDelayed(i * 0.1f, () =>
				{
					PhysicsDice dice = dices[index];

					dice.SetSimulatable(false);
					dice.Stop();
					
					dice.transform.SetParent(viewPosition[index]);

					Tweener mover = dice.transform.DOLocalMove(Vector3.zero, 0.4f);

					Vector3 rotation = Vector3.zero;
					switch (dice.GetComponent<PhysicsDice>().DiceValue)
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
						dice.transform.SetParent(transform);
					});
				});
			}
		}

		public void RestoreDice()
		{
			for (int i = 0; i < dices.Length; i++)
			{
				PhysicsDice dice = dices[i];

				if (dice.IsLocked)
					continue;

				dice.transform.DOMove(dice.PlacedPosition, 0.4f);
				dice.transform.DORotate(dice.PlacedRotation.eulerAngles, 0.4f);
			}
		}

		public void ReplayDice()
		{
			StartCoroutine(Replaying());
		}

		private IEnumerator Replaying()
		{
			float elapsedTime = recordEndTime - recordStartTime;
			float t = 0.0f;
			while (true)
			{
				yield return null;
				foreach (PhysicsDice dice in dices)
				{
					dice.Replay(t);
				}

				t += Time.deltaTime * Time.timeScale;
				
				if (t > elapsedTime)
				{
					Debug.Log($"리플레이 종료");
					break;
				}
			}
		}


		private void OnLockStateChanged(bool isLocked)
		{
			
		}
	}
}