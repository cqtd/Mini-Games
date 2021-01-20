using UnityEngine;
using UnityEngine.UI;

namespace Yacht.UIToolkit
{
	public class ScoreCell : MonoBehaviour
	{
		public Button button = default;
		public Text text = default;

		private void Reset()
		{
			button = GetComponent<Button>();
			text = GetComponentInChildren<Text>();
		}
	}
}