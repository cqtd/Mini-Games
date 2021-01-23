using System;
using UnityEngine;
using UnityEngine.UI;

namespace Yacht.UIToolkit
{
	public class TitleCanvas : MonoBehaviour
	{
		public Button startButton = default;
		public Button howToButton = default;
		public Button creditButton = default;

		private void OnEnable()
		{
			startButton.onClick.AddListener(OnStart);
			howToButton.onClick.AddListener(OnHowTo);
			creditButton.onClick.AddListener(OnCredit);
		}

		private void OnDisable()
		{
			startButton.onClick.RemoveListener(OnStart);
			howToButton.onClick.RemoveListener(OnHowTo);
			creditButton.onClick.RemoveListener(OnCredit);
		}

		private void OnStart()
		{
			CanvasManager.Instance.ShowGameView();
		}

		private void OnHowTo()
		{
			
		}

		private void OnCredit()
		{
			
		}
	}
}