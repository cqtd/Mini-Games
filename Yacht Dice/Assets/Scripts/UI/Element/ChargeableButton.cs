using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	[RequireComponent(typeof(Graphics))]
	[ExecuteAlways]
	[SelectionBase]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/ChargeableButton", 71)]
	public class ChargeableButton : Selectable
	{
		[Serializable]
		public class PressStartEvent : UnityEvent { }

		[Serializable]
		public class PressEndEvent : UnityEvent { }

		[Serializable]
		public class PressStayEvent : UnityEvent { }

		[SerializeField] protected PressStartEvent m_onPressStart = new PressStartEvent();
		[SerializeField] protected PressEndEvent m_onPressEnd = new PressEndEvent();
		[SerializeField] protected PressStayEvent m_onPressStay = new PressStayEvent();

		public bool IsPressing { get; protected set; }

		public PressStartEvent onPressStart {
			get
			{
				return m_onPressStart;
			}
		}

		public PressEndEvent onPressEnd {
			get
			{
				return m_onPressEnd;
			}
		}

		public PressStayEvent onPressStay {
			get
			{
				return m_onPressStay;
			}
		}
		
		protected override void Reset()
		{
			base.Reset();
			
			m_onPressStart = new PressStartEvent();
			m_onPressEnd = new PressEndEvent();
			m_onPressStay = new PressStayEvent();
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			
			base.OnPointerExit(eventData);
			
			if (!interactable) return;
			if (!IsPressing) return;
			
			IsPressing = false;
			m_onPressEnd?.Invoke();
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			
			if (!interactable) return;

			IsPressing = true;
			m_onPressStart.Invoke();
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			
			if (!interactable) return;
			if (!IsPressing) return;
			
			IsPressing = false;
			
			m_onPressEnd?.Invoke();
		}

		protected virtual void Update()
		{
			if (IsPressing)
			{
				onPressStay.Invoke();
			}
		}
	}
}