using CQ.UI;
using DG.Tweening;
using UnityEngine;

namespace CQ.MiniGames
{
	public class GameModeBox : UIElement
	{
		public RectTransform hideTarget = default;

		Vector3 hidePos = default;
		Vector3 showPos = default;
		
		public void InitComponent()
		{
			showPos = transform.position;
			hidePos = hideTarget.transform.position;
			
			transform.position = hidePos;
		}

		public void Show()
		{
			transform.DOMove(showPos, Duration.VeryFast);
		}

		public void Hide()
		{
			transform.DOMove(hidePos, Duration.VeryFast);
		}
	}
}