using CQ.UI;
using DG.Tweening;
using UnityEngine;

namespace CQ.MiniGames
{
	public class SlideWindow : UIWindow
	{
		RectTransform rect = default;
		
		public override void InitComponent()
		{
			rect = transform as RectTransform;
		}
		
		public override void Open()
		{
			rect.DOAnchorPosX(-rect.sizeDelta.x, Duration.Fast);
		}
		
		public override void Close()
		{
			rect.DOAnchorPosX(0, Duration.Fast);
		}
	}
}