using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CQ.MiniGames
{
	public class DiceRollTest : MonoBehaviour
	{
		enum EDiceState
		{
			NONE = 0,
			
			FREE,
			SELECT,
			LOCKED,
			
			COUNT,
		}
		
		public Rigidbody dicePrefab = default;
		public Transform startPosMarker = default;

		public float minOffset = -1;
		public float maxOffset = 1;

		public float minForce = 2;
		public float maxForce = 10;

		public float minAngular = 0;
		public float maxAnguler = 360;

		Rigidbody[] dices;
		EDiceState[] states;

		public Transform[] lockedPosition;

		List<Tweener> tweens;
		
		void Awake()
		{
			tweens = new List<Tweener>();
			
			dices = new Rigidbody[5];
			for (int i = 0; i < 5; i++)
			{
				dices[i] = Instantiate(dicePrefab, transform);
			}
			
			Roll();
		}

		void Start()
		{
			
		}

		Vector3 GetRandomOffset(float min, float max)
		{
			return new Vector3(Random.Range(min, max), Random.Range(min, max),Random.Range(min, max));
		}

		[ContextMenu("Roll")]
		public void Roll()
		{
			foreach (Tweener tween in tweens)
			{
				tween?.Kill(true);
			}

			int completeCount = 0;
			
			foreach (Rigidbody dice in dices)
			{
				dice.transform.position = startPosMarker.position + GetRandomOffset(minOffset, maxOffset);

				dice.isKinematic = false;
				dice.useGravity = true;
				dice.angularVelocity = GetRandomOffset(minAngular, maxAnguler);
				dice.velocity = startPosMarker.forward * Random.Range(minForce, maxForce);
				
				dice.GetComponent<DiceCube>().BeginSimulate(() =>
				{
					completeCount++;
					if (dices.Length == completeCount)
					{
						Timing.CallDelayed(3.0f, MoveDices);
						// MoveDices();
					}
				});
			}
		}

		IEnumerator WaitForStop()
		{
			foreach (Rigidbody dice in dices)
			{
				if (dice.velocity.sqrMagnitude > 0.01f)
					yield return null;
			}
			
			yield return new WaitForSeconds(5);
			
			MoveDices();
		}

		void MoveDices()
		{
			for (int i = 0; i < dices.Length; i++)
			{
				Rigidbody dice = dices[i];
				
				dice.useGravity = false;
				dice.transform.SetParent(lockedPosition[i]);
				
				var mover = dice.transform.DOLocalMove(Vector3.zero, 0.4f);
				var rotater = dice.transform.DOLocalRotate(Vector3.zero, 0.4f);
				mover.OnComplete(() =>
				{
					tweens.Remove(mover);
				});
				
				rotater.OnComplete(() =>
				{
					dice.transform.SetParent(transform);
					tweens.Remove(rotater);
				});
				
				tweens.Add(mover);
				tweens.Add(rotater);
				
				dice.velocity = Vector3.zero;
				dice.angularVelocity = Vector3.zero;
				dice.isKinematic = true;
			}
		}

		// @TODO 1 :: 애니메이션 베이킹 시스템 구현
		// @TODO 2 :: 굴리기 애니메이션 - 락인포지션 이동 트윈으로 구현
		// @TODO 3 :: 락인 구현
		
		void Update()
		{
			
		}

		void BeginRecord()
		{
			
		}
	}
}