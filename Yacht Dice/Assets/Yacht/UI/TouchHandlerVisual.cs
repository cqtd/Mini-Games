using System;
using CQ.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class TouchHandlerVisual : UIWindow
	{
		private TouchHandler handler = default;
		
		[SerializeField] private Image startPosition = default;
		[SerializeField] private Image endedPosition = default;

		private Vector2 scaler;

		private void Awake()
		{
			// handler.onTouchBegan += OnTouchBegan;
			// handler.onTouchEnded += OnTouchEnded;
			// handler.onTouchStay += OnTouchStay;
		}

		private void OnTouchBegan(Vector2 position)
		{
			startPosition.rectTransform.anchoredPosition = position * scaler;
		}

		private void OnTouchEnded(Vector2 position)
		{
			endedPosition.rectTransform.anchoredPosition = position * scaler;
		}

		private void OnTouchStay(Vector2 position)
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

		private void OnScreenSizeChangeCallback(Vector2 resolution)
		{
			Debug.Log($"{resolution.x}, {resolution.y}");

			scaler = YachtUIManager.Instance.Ratio;
		}
	}
}