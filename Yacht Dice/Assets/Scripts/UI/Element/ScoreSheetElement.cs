using System;
using CQ.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class ScoreSheetElement : UIElement
	{
		[SerializeField] private TextMeshProUGUI m_elementName = default;
		[SerializeField] private TextMeshProUGUI m_filledAmount = default;
		
		[SerializeField] private Button m_fill = default;

		[SerializeField] private string m_name = default;
		private bool isFilled;

		public void InitComponent()
		{
			m_elementName.SetText(m_name);
			m_filledAmount.SetText("");
			
			m_fill.interactable = false;
		}

		public void AddListener(UnityAction action)
		{
			m_fill.onClick.AddListener(action);
		}

		public void SetValue(int value)
		{
			m_filledAmount.SetText($"{value}");
			isFilled = true;
			
			Deactivate();
		}

		public void SetPreview(int value)
		{
			if (!isFilled)
			{
				m_filledAmount.SetText($"{value}");
			}
		}

		public void ClearPreview()
		{
			if (!isFilled)
			{
				m_filledAmount.SetText("");
			}
		}


		public void Activate()
		{
			if (!isFilled)
				m_fill.interactable = true;
		}

		public void Deactivate()
		{
			m_fill.interactable = false;
		}
	}
}