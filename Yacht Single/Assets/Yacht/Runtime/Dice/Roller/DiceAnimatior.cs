using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MEC;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Yacht.ReplaySystem
{
	using AssetManagement;
	using Gameplay;

	public class DiceAnimatior : MonoBehaviour
	{
		/// <summary>
		/// 롤링 애니메이션이 시작될 때
		/// </summary>
		public event Action onAnimationBegin;
		/// <summary>
		/// 롤링 애니메이션이 끝날 때
		/// </summary>
		public event Action onAnimationEnd;
		/// <summary>
		/// 다이스 시뮬레이션 애니메이션이 멈췄을 때
		/// </summary>
		public event Action onDiceStopped;
		
		[SerializeField] private float m_viewDiceInterval = 1.0f;
		
		private VisualDice[] diceEntities = default;
		private Coroutine playCoroutine;
		private bool isPaused = false;
		private bool isPlaying = false;

		private bool isReady;

		#region API
		
		public void Initialize()
		{

		}
		
		public bool IsPlaying()
		{
			return isPlaying;
		}
		
		public void Play(List<Dice> dices)
		{
			if (!Patchable.Instance.IsAnimationLoaded) return;
			if (isPlaying)
			{

				Debug.Log("재생 중입니다. 애니메이션 종료를 기다려주세요.");

				return;
			}

			onAnimationBegin?.Invoke();
			
			int lockedCount = dices.Count(e => e.IsHolding());

			List<RollingAnimation> animationPack = Patchable.Instance.animationMap[5 - lockedCount];
			RollingAnimation anim = animationPack[Random.Range(0, animationPack.Count - 1)];

			playCoroutine = StartCoroutine(PlayCoroutine(anim, dices));
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
			}

			isPaused = false;
			isPlaying = false;
		}
		
		#endregion

		#region INTERNAL
		
		private void Awake()
		{
			diceEntities = new VisualDice[Constants.NUM_DICES];
			
			StartCoroutine(LoadDicePrefabCoroutine());
		}

		private VisualDice dicePrefab = default;

		private IEnumerator LoadDicePrefabCoroutine()
		{
			yield return new WaitUntil(() => Patchable.Instance.isAddressableReady);
			var operation = Addressables.LoadAssetAsync<GameObject>("Assets/Patchable/Dice/Dice_001_A.prefab");

			yield return operation;

			dicePrefab = operation.Result.GetComponent<VisualDice>();
			isReady = true;
			
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				diceEntities[i] = Instantiate<VisualDice>(dicePrefab, transform);
				diceEntities[i].gameObject.SetActive(false);
			}
			
			for (int i = 0; i < diceEntities.Length; i++)
			{
				diceEntities[i].Initialize(
					Game.Instance.Player[i],
					World.ViewPosition[i],
					World.HoldPosition[i]
				);
			}

			Game.Instance.Player.onScoreChange += OnScoreChange;
		}

		private IEnumerator PlayCoroutine(RollingAnimation rollAnim, IReadOnlyList<Dice> dices)
		{
			isPlaying = true;

			// 주사위들을 애니메이션 시작 위치로 이동시킵니다.
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				if (diceEntities[i].IsLocked) continue;

				diceEntities[i].TweenStartPos(0.4f);
			}

			// 이동 대기
			yield return new WaitForSeconds(0.5f);

			int animIndex = 0;
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				if (diceEntities[i].IsLocked) continue;
				
				this.diceEntities[i].gameObject.SetActive(true);

				this.diceEntities[i].rotationOffset = rollAnim.datas[animIndex].upside.GetLocalRotation((Enums.DiceValue) dices[i].GetValue());
				animIndex++;
			}
			
			// 재생 로직
			float elapsedTime = rollAnim.length;
			float t = 0.0f;
			
			while (true)
			{
				int animIndex2 = 0;
				for (int i = 0; i < Constants.NUM_DICES; i++)
				{
					if (diceEntities[i].IsLocked) continue;
					
					rollAnim.datas[animIndex2].Set(t, this.diceEntities[i].transform, this.diceEntities[i].rotationOffset);
					animIndex2++;
				}

				t += Time.deltaTime * Time.timeScale;
				
				// 재생 끝
				if (t > elapsedTime)
				{
					// Debug.Log($"리플레이 종료");
					break;
				}

				// 일시정지
				while (isPaused)
				{
					yield return null;
				}

				yield return null;
			}
			
			// 클린
			isPlaying = false;
			
			onDiceStopped?.Invoke();

			yield return new WaitForSeconds(m_viewDiceInterval);
			ViewDice();

			int estimated = 0;
			int current = 0;
			
			for (int i = 0; i < diceEntities.Length; i++)
			{
				int index = i;

				if (diceEntities[i].IsLocked) continue;

				estimated++;
				VisualDice dice = diceEntities[index];
				dice.CacheTransform();
				
				Timing.CallDelayed(index * 0.1f, () =>
				{
					dice.TweenView(0.4f, () => current++);
				});
			}

			yield return new WaitUntil(() => current == estimated);
			
			onAnimationEnd?.Invoke();
		}

		private void ViewDice()
		{
			for (int i = 0; i < diceEntities.Length; i++)
			{
				int index = i;
				
				if (diceEntities[i].IsLocked) continue;
				
				VisualDice dice = diceEntities[index];
				dice.CacheTransform();

				Timing.CallDelayed(index * 0.1f, () =>
				{
					dice.TweenView(0.4f);
				});
			}
		}

		private void OnScoreChange()
		{
			for (int i = 0; i < diceEntities.Length; i++)
			{
				diceEntities[i].TweenStartPos(0.4f);
				diceEntities[i].ChangeColor(false);
			}
		}

		#endregion
	}
	
	public enum EDiceState
	{
		NONE,
			
		/// <summary>
		/// 애니메이션 중
		/// </summary>
		ANIMATING,

		/// <summary>
		/// 뷰-홀드-스타팅 트랜지션 중
		/// </summary>
		TRANSITION,

		/// <summary>
		/// 애니메이션 후 테이블 위
		/// </summary>
		TABLE,
		/// <summary>
		/// 뷰 포지션 고정
		/// </summary>
		VIEW,
		/// <summary>
		/// 홀드 포지션 고정
		/// </summary>
		HOLD,
			
		/// <summary>
		/// 스타팅 포지션 고정
		/// </summary>
		STARTING,
	}
}