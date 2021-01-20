using System;
using UnityEngine;
using UnityEngine.UI;
using Yacht.ReplaySystem;

namespace Yacht.UIToolkit
{
	public class GameCanvas : MonoBehaviour
	{
		public Button rollButton;
		public Button fillButton;
		public Text rollText;
		
		public ScoreCell[] scores = default;
		public DiceAnimatior dicer;

		public void Repaint()
		{
			foreach (Enums.Category category in Enum.GetValues(typeof(Enums.Category)))
			{
				if (Game.Instance.Player.GetScoresheet().IsEmpty(category))
				{
					scores[(int) category].button.interactable = true;
					scores[(int) category].text.text = Game.Instance.Player.GetEstimatedScore(category).ToString();
				}
				else
				{
					scores[(int) category].button.interactable = false;
					scores[(int) category].text.text =
						Game.Instance.Player.GetScoresheet().GetScore(category).ToString();
				}
			}

			rollButton.interactable = Game.Instance.Player.CanRoll();
			rollText.text = $"Roll ({3-Game.Instance.Player.GetChance()}/3)";
		}

		private void OnEnable()
		{
			rollButton.onClick.AddListener(Roll);
		}

		private void OnDisable()
		{
			rollButton.onClick.RemoveListener(Roll);
		}

		public void Roll()
		{
			if (Game.Instance.Player.CanRoll())
			{
				Game.Instance.Player.RollDices(dicer.GetUnlockedIndecies());
				dicer.Play(Game.Instance.Player.GetDiceValues());
			}
			else
			{
			}
			
			Repaint();
		}
	}
}