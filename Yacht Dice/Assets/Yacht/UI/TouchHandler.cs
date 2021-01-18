using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ.MiniGames
{
	public class TouchHandler : MonoBehaviour
	{
		public float swiperSensitivity = 1.0f;
		
		[Tooltip("진동 길이 (단위 : ms)")]
		public long vibrationDuration = 300L;
		[Tooltip("진동 인터벌 (단위 : ms)")]
		public long vibrationInterval = 100L;

		private Dictionary<int, Vector2> touchBeganPositionMap;
		private Dictionary<int, Vector2> touchEndedPositionMap;

		private bool m_isMouseDown = false;
		private bool m_isForcingFeedback = false;

		public event Action<Vector2> onTouchBegan; 
		public event Action<Vector2> onTouchEnded; 
		public event Action<Vector2> onTouchStay;

		private void Awake()
		{
			touchBeganPositionMap = new Dictionary<int, Vector2>();
			touchEndedPositionMap = new Dictionary<int, Vector2>();

			Vibration.Init();
		}

		private void Update()
		{
			var touchCount = Input.touchCount;
			
			if (touchCount > 0)
			{
				for (int i = 0; i < touchCount; i++)
				{
					var touch = Input.GetTouch(i);

					if (touch.phase == TouchPhase.Began)
					{
						// Begin Touch
						OnTouchBegan(touch, i);
					}
					
					else if (touch.phase == TouchPhase.Ended)
					{
						// End of touch
						
						OnTouchEnded(touch, i);
					}
					
					else if (touch.phase == TouchPhase.Canceled)
					{
						// Cancel of touch
					}
					
					else if (touch.phase == TouchPhase.Stationary)
					{
						OnTouchStay(touch, i);
					}
					
					else if (touch.phase == TouchPhase.Moved)
					{
						OnTouchMoved(touch, i);
					}
				}
			}

			if (Input.GetMouseButtonDown(0))
			{
				m_isMouseDown = true;
				// Debug.Log($"Touch Began : {Input.mousePosition.x}, {Input.mousePosition.y}");
				
				onTouchBegan?.Invoke(Input.mousePosition);
			}

			if (Input.GetMouseButtonUp(0))
			{
				m_isMouseDown = false;
				// Debug.Log($"Touch Ended : {Input.mousePosition.x}, {Input.mousePosition.y}");
				
				onTouchEnded?.Invoke(Input.mousePosition);
			}

			if (m_isMouseDown)
			{
				onTouchStay?.Invoke(Input.mousePosition);
			}
		}

		private void OnTouchBegan(Touch touch, int index)
		{
			touchBeganPositionMap[index] = touch.position;
			
			onTouchBegan?.Invoke(touchBeganPositionMap[index]);
			StartCoroutine(ForceFeedback());
			
			// Debug.Log($"Touch Began : {touchBeganPositionMap[index].x}, {touchBeganPositionMap[index].y}");
		}

		private void OnTouchEnded(Touch touch, int index)
		{
			touchEndedPositionMap[index] = touch.position;
			Vector2 deltaValue = touchEndedPositionMap[index] - touchBeganPositionMap[index];
			
			onTouchEnded?.Invoke(touchEndedPositionMap[index]);

			m_isForcingFeedback = false;
			
			// Debug.Log($"Touch Ended : {touchEndedPositionMap[index].x}, {touchEndedPositionMap[index].y}");
		}

		private void OnTouchStay(Touch touch, int index)
		{
			// Vibration.VibratePop();
		}

		private void OnTouchMoved(Touch touch, int index)
		{
			// Vibration.VibratePop();
		}

		private IEnumerator ForceFeedback()
		{
			while (m_isForcingFeedback)
			{
				Vibration.Vibrate(vibrationDuration);
				yield return new WaitForSeconds(vibrationInterval * 0.001f);
			}
		}
	}
}