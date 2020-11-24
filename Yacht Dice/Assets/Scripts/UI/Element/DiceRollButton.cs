using System;
using CQ.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CQ.MiniGames.UI
{
	[RequireComponent(typeof(Graphics))]
	public class DiceRollButton : UIElement, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		public bool interactable = true;

		bool isPressing = false;
		
		public event Action onPressStart = default;
		public event Action onPressEnd = default;
		
		public void OnPointerDown(PointerEventData eventData)
		{
			if (!interactable) return;

			isPressing = true;
			onPressStart.Invoke();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (!interactable) return;
			if (!isPressing) return;
			
			isPressing = false;
			
			onPressEnd?.Invoke();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!interactable) return;
			if (!isPressing) return;
			
			isPressing = false;
			onPressEnd?.Invoke();
		}
	}
}