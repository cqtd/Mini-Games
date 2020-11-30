using CQ.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class TopWindow : UIWindow
	{
		[SerializeField] private Button shop = default;
		[SerializeField] private Button events = default;
		[SerializeField] private Button notice = default;
		
		public Button slide = default;

		public UnityEvent onSlideBegin = default;
		
		public override void InitComponent()
		{
			slide.onClick.AddListener(OnSlideBegin);
			
			Reference.Use(shop);
			Reference.Use(events);
			Reference.Use(notice);
		}

		private void OnSlideBegin()
		{
			onSlideBegin?.Invoke();
		}
	}
}
