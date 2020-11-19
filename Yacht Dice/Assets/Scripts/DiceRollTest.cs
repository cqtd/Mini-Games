using System.Collections;
using DG.Tweening;
using MEC;
using UnityEngine;

namespace CQ.MiniGames
{
	using ReplaySystem;
	
	public class DiceRollTest : MonoBehaviour
	{
		public DiceCube dicePrefab = default;
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

		DiceCube[] dices;

		const string pathFormat = "Assets/Animations/Dice {0}/recorded_{1}.asset";

		void Awake()
		{
			dices = new DiceCube[5];
			for (int i = 0; i < 5; i++)
			{
				dices[i] = Instantiate(dicePrefab, transform);
				dices[i].onLockStateChanged += OnLockStateChanged;
				dices[i].name = $"(Instance) {dicePrefab.name} {i:00}";
			}
			
			RollDice();
		}

		Vector3 GetRandomOffset(float min, float max)
		{
			return new Vector3(Random.Range(min, max), Random.Range(min, max),Random.Range(min, max));
		}

		float recordStartTime;
		float recordEndTime;

		public void RollDice()
		{
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
			
			foreach (DiceCube dice in dices)
			{
				if (dice.IsLocked)
				{
					// dice.transform.DOLocalMove(-2 * Vector3.up, 0.4f);
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
					dice.GetComponent<ReplayEntity>().Record();
					
					recordStartTime = Time.time;
					
					// dice.transform.position = startPosMarker.position + GetRandomOffset(minOffset, maxOffset);

					dice.SetCollidable(true);
					dice.SetSimulatable(true);
					dice.SetVelocity(velocity, angular);
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
					DiceCube dice = dices[index];

					dice.SetSimulatable(false);
					dice.Stop();
					
					dice.transform.SetParent(viewPosition[index]);

					Tweener mover = dice.transform.DOLocalMove(Vector3.zero, 0.4f);

					Vector3 rotation = Vector3.zero;
					switch (dice.GetComponent<DiceCube>().DiceValue)
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
				DiceCube dice = dices[i];

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

		IEnumerator Replaying()
		{
			float elapsedTime = recordEndTime - recordStartTime;
			float t = 0.0f;
			while (true)
			{
				yield return null;
				foreach (DiceCube dice in dices)
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
		
		
		void OnLockStateChanged(bool isLocked)
		{
			
		}

		// @TODO 1 :: 애니메이션 베이킹 시스템 구현
		// @TODO 2 :: 굴리기 애니메이션 - 락인포지션 이동 트윈으로 구현
		// @TODO 3 :: 락인 구현
	}
}