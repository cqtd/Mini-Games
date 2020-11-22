using System;
using CQ.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class TouchHandlerVisual : UIWindow
	{
		TouchHandler handler = default;
		
		[SerializeField] Image startPosition = default;
		[SerializeField] Image endedPosition = default;

		Vector2 scaler;

		void Awake()
		{
			// handler.onTouchBegan += OnTouchBegan;
			// handler.onTouchEnded += OnTouchEnded;
			// handler.onTouchStay += OnTouchStay;
		}

		void OnTouchBegan(Vector2 position)
		{
			startPosition.rectTransform.anchoredPosition = position * scaler;
		}
		
		void OnTouchEnded(Vector2 position)
		{
			endedPosition.rectTransform.anchoredPosition = position * scaler;
		}
		
		void OnTouchStay(Vector2 position)
		{
			
		}

		public override void InitComponent()
		{
			handler = FindObjectOfType<TouchHandler>();
			
			handler.onTouchBegan += OnTouchBegan;
			handler.onTouchEnded += OnTouchEnded;
			handler.onTouchStay += OnTouchStay;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			YachtUIManager.UnregisterScreenSizeChange(OnScreenSizeChangeCallback);
		}
		
		
		protected override void OnEnable()
		{
			base.OnEnable();
			
			YachtUIManager.RegisterScreenSizeChange(OnScreenSizeChangeCallback);
		}

		void OnScreenSizeChangeCallback(Vector2 resolution)
		{
			Debug.Log($"{resolution.x}, {resolution.y}");

			scaler = YachtUIManager.Instance.Ratio;
		}
	}
}