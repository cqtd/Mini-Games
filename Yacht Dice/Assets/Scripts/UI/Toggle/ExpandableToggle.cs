using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	[RequireComponent(typeof(Toggle))]
	public class ExpandableToggle : MonoBehaviour
	{
		public TextMeshProUGUI openText = default;
		public Toggle toggle = default;
		public RectTransform childRoot = default;
		private RectTransform rt = default;

		private void Reset()
		{
			toggle = GetComponent<Toggle>();
		}

		private void Awake()
		{
			rt = transform as RectTransform;

			openText.SetAlpha(0);
			
			toggle.onValueChanged.AddListener(OnValueChanged);
		}

		private void OnValueChanged(bool isOn)
		{
			if (isOn)
			{
				openText.DOFade(1.0f, 0.5f);
				var sizeY = (childRoot.childCount + 1) * 200f;
				rt.DOSizeDelta(new Vector2(rt.sizeDelta.x, sizeY), 0.5f);
			}
			else
			{
				openText.DOFade(0.0f, 0.5f);
				rt.DOSizeDelta(new Vector2(rt.sizeDelta.x, 200f), 0.5f);
			}
		}
	}
}