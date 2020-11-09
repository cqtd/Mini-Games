﻿using CQ.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CQ.MiniGames
{
	public class TopWindow : UIWindow
	{
		[SerializeField] Button shop = default;
		[SerializeField] Button events = default;
		[SerializeField] Button notice = default;
		
		public Button slide = default;

		public UnityEvent onSlideBegin = default;
		
		public override void InitComponent()
		{
			slide.onClick.AddListener(OnSlideBegin);
		}

		void OnSlideBegin()
		{
			onSlideBegin?.Invoke();
		}
	}
}
