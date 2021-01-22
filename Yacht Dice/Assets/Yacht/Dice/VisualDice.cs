using System;
using CQ.MiniGames;
using DG.Tweening;
using UnityEngine;
using Yacht.Gameplay;
using Random = UnityEngine.Random;

namespace Yacht.ReplaySystem
{
	public class VisualDice : DiceBase
	{

		
		public Vector3 rotationOffset;
		private Dice entity = default;

		private Transform viewPosition = default;
		private Transform holdPosition = default;

		public EDiceState viewState = EDiceState.NONE;

		public override int DiceValue {
			get
			{
				return entity.GetValue();
			} 
		}

		public override bool IsLocked {
			get
			{
				return entity.IsHolding();
			}
			set
			{
				if (value) entity.Hold();
				else entity.Unhold();
			}
		}

		protected override void Reset()
		{
			
		}

		public void Initialize(Dice dice, Transform view, Transform hold)
		{
			this.viewPosition = view;
			this.holdPosition = hold;
			
			Bind(dice);
		}

		private void Bind(Dice dice)
		{
			this.entity = dice;

			dice.onDiceLocked += OnDiceLocked;
			dice.onDiceRolled += OnDiceRolled;
		}

		private void Unbind(Dice dice)
		{
			this.entity = dice;
			
			dice.onDiceLocked -= OnDiceLocked;
			dice.onDiceRolled -= OnDiceRolled;
		}

		private void OnMouseDown()
		{
			switch (viewState)
			{
				case EDiceState.NONE:
				case EDiceState.ANIMATING:
				case EDiceState.TRANSITION:
				case EDiceState.TABLE:
				case EDiceState.STARTING:
					return;
				case EDiceState.VIEW:
				case EDiceState.HOLD:
					ChangeLockState(!IsLocked);
					break;
			}
		}

		private void ChangeLockState(bool locked)
		{
			IsLocked = locked;

			if (IsLocked)
			{
				viewState = EDiceState.TRANSITION;
				Tweener tween = transform.DOMove(holdPosition.position, 0.4f);
				tween.OnComplete(() => viewState = EDiceState.HOLD);
			}
			else
			{
				viewState = EDiceState.TRANSITION;
				Tweener tween = transform.DOMove(viewPosition.position, 0.4f);
				tween.OnComplete(() => viewState = EDiceState.VIEW);
			}
		}

		private void OnDiceLocked(bool locked)
		{
			RefreshColor();
		}

		private void OnDiceRolled(int diceValue)
		{
			
		}

		private Transform prevParent = default;

		public void TweenView(float duration, Action onComplete = null)
		{
			viewState = EDiceState.TRANSITION;
			
			prevParent = transform.parent;
			transform.SetParent(viewPosition);

			Vector3 rotation = Vector3.zero;
			switch (DiceValue)
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
			}
			transform.DOLocalMove(Vector3.zero, duration);

			Tweener rotater = transform.DOLocalRotate(rotation, duration);
			rotater.OnComplete(() =>
			{
				transform.SetParent(prevParent);
				viewState = EDiceState.VIEW;
				
				onComplete?.Invoke();
			});
		}


		public void TweenStartPos(float duration)
		{
			transform.DOMove(World.StartPosition.position, duration);
			Tweener tween = transform.DORotate(Random.onUnitSphere * 180f, duration);
			tween.OnComplete(() => viewState = EDiceState.STARTING);
		}
	}
}