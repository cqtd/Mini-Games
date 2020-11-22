using System;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class TouchHandlerVisual : MonoBehaviour
	{
		[SerializeField] TouchHandler handler = default;
		[SerializeField] Image startPosition = default;
		[SerializeField] Image endedPosition = default;

		void Awake()
		{
			handler.onTouchBegan += OnTouchBegan;
			handler.onTouchEnded += OnTouchEnded;
			handler.onTouchStay += OnTouchStay;
		}

		void OnTouchBegan(Vector2 position)
		{
			startPosition.rectTransform.anchoredPosition = position;
		}
		
		void OnTouchEnded(Vector2 position)
		{
			endedPosition.rectTransform.anchoredPosition = position;
		}
		
		void OnTouchStay(Vector2 position)
		{
			
		}
	}
}