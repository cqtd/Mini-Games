using UnityEngine;
using UnityEngine.UI;
using Yacht.Gameplay;
using Yacht.ReplaySystem;

namespace Yacht.UIToolkit
{
	public class GameCanvas : MonoBehaviour
	{
		public Button rollButton;
		public Button fillButton;
		public Text rollText;
		
		public ScoreCell[] scores = default;
		
		public Text upperScore = default;
		public Text upperBonus = default;
		public Text totalScore = default;
		
		public DiceAnimatior dicer;

		public void Initialize()
		{
			foreach (ScoreCell scoreCell in scores)
			{
				scoreCell.Initialize();
				scoreCell.Repaint(ScoreCell.EPhase.CANNOT_FILL);
			}

			rollButton.interactable = Game.Instance.Player.CanRoll();
			rollText.text = $"Roll ({3-Game.Instance.Player.GetChance()}/3)";
			
			upperScore.text = Game.Instance.Player.Scoresheet.GetUpperPoint().ToString();
			upperBonus.text = $"+{Game.Instance.Player.Scoresheet.GetBonusPoint().ToString()}";
			totalScore.text = Game.Instance.Player.Scoresheet.GetTotalScore().ToString();
		}

		private void OnEnable()
		{
			rollButton.onClick.AddListener(OnClick_RollButton);
			
			Game.Instance.Player.onScoreChange += OnScoreChange;
			Game.Instance.Player.onSessionComplete += OnSessionComplete;

			dicer.onAnimationBegin += OnAnimationBegin;
			dicer.onAnimationEnd += OnAnimationEnd;

			foreach (Dice dice in Game.Instance.Player.GetDices())
			{
				dice.onDiceLocked += OnDiceLockChanged;
			}
		}

		private void OnDisable()
		{
			rollButton.onClick.RemoveListener(OnClick_RollButton);
			
			Game.Instance.Player.onScoreChange -= OnScoreChange;
			Game.Instance.Player.onSessionComplete -= OnSessionComplete;

			dicer.onAnimationBegin -= OnAnimationBegin;
			dicer.onAnimationEnd -= OnAnimationEnd;
			
			foreach (Dice dice in Game.Instance.Player.GetDices())
			{
				dice.onDiceLocked -= OnDiceLockChanged;
			}
		}
		
		private void OnAnimationBegin()
		{
			rollButton.interactable = false;

			foreach (ScoreCell scoreCell in scores)
			{
				scoreCell.Repaint(ScoreCell.EPhase.CANNOT_FILL);
			}
		}

		private void OnAnimationEnd()
		{
			rollButton.interactable = Game.Instance.Player.CanRoll();
			
			foreach (ScoreCell scoreCell in scores)
			{
				scoreCell.Repaint(ScoreCell.EPhase.CAN_FILL);
			}
		}

		private void OnDiceLockChanged(bool hold)
		{
			rollButton.interactable = Game.Instance.Player.CanRoll();
		}

		private void OnScoreChange()
		{
			foreach (ScoreCell scoreCell in scores)
			{
				scoreCell.Repaint(ScoreCell.EPhase.CANNOT_FILL);
			}

			rollButton.interactable = Game.Instance.Player.CanRoll();
			rollText.text = $"Roll ({3 - Game.Instance.Player.GetChance()}/3)";

			upperScore.text = Game.Instance.Player.Scoresheet.GetUpperPoint().ToString();
			upperBonus.text = $"+{Game.Instance.Player.Scoresheet.GetBonusPoint().ToString()}";
			totalScore.text = Game.Instance.Player.Scoresheet.GetTotalScore().ToString();
		}

		private void OnClick_RollButton()
		{
			if (Game.Instance.Player.CanRoll())
			{
				Game.Instance.Player.Roll();
				dicer.Play(Game.Instance.Player.GetDices());
			}
			
			rollText.text = $"Roll ({3-Game.Instance.Player.GetChance()}/3)";
		}
		
		private void OnSessionComplete()
		{
			
		}
	}
}