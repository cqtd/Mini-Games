using UnityEngine;
using UnityEngine.UI;
using Yacht.Gameplay;

namespace Yacht.UIToolkit
{
	public class ScoreCell : MonoBehaviour
	{
		public enum EPhase
		{
			NONE,
			
			CAN_FILL,
			CANNOT_FILL,
		}
		
		[SerializeField] private Button button = default;
		[SerializeField] private Text text = default;
		[SerializeField] private Enums.Category category = default;

		private Player player = default;
		private Scoresheet scoresheet = default;

		private void Reset()
		{
			button = GetComponent<Button>();
			text = GetComponentInChildren<Text>();
		}

		private void OnEnable()
		{
			button.onClick.AddListener(OnClick);
		}

		private void OnDisable()
		{
			button.onClick.RemoveListener(OnClick);
		}

		private void OnClick()
		{
			Game.Instance.Player.FillScoreSheet(category);
		}
		
		public void Initialize()
		{
			scoresheet = Game.Instance.Player.Scoresheet;
			player = Game.Instance.Player;
		}

		public void Repaint(EPhase phase)
		{
			//  비어있는 칸
			if (scoresheet.IsEmpty(category))
			{
				// 예상되는 점수 표시
				if (phase == EPhase.CAN_FILL)
				{
					text.text = $"{player.GetEstimatedScore(category)}";

					button.image.enabled = true;
					button.interactable = true;
				}
				// 다 지우기
				else
				{
					button.interactable = false;
					button.image.enabled = false;
					
					text.text = "";
				}
			}
			// 값이 들어있는 칸
			else
			{
				button.interactable = false;
				button.image.enabled = false;

				text.text = $"{scoresheet[category]}";
			}
		}
	}
}