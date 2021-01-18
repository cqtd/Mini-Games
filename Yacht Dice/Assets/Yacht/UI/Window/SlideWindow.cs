using CQ.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class SlideWindow : UIWindow
	{
		private RectTransform rect = default;

		public Image avatar = default;

		private const string path = "Assets/Sprites/Mockups/Orono Noguchi 1.png";
		
		public override void InitComponent()
		{
			rect = transform as RectTransform;

			AsyncOperationHandle<Sprite> operation = Addressables.LoadAssetAsync<Sprite>(path);
			operation.Completed += handle =>
			{
				avatar.sprite = handle.Result;
			};
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