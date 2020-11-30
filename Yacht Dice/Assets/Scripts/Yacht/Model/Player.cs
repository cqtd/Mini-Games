using System;
using System.Collections.Generic;
using System.Linq;

namespace CQ.MiniGames.Yacht
{
	public class Player
	{
		private readonly List<Dice> m_dices;
		private readonly Dictionary<Enums.Category, int> m_scores;

		private Scoresheet m_scoresheet;
		private int m_reroll;

		public Player()
		{
			m_dices = new List<Dice>(Constants.NUM_DICES);
			m_scores = new Dictionary<Enums.Category, int>(Constants.NUM_SCORES);
		}

		public void Initialize()
		{
			m_scoresheet = new Scoresheet();
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				m_dices.Add(new Dice(DateTime.Now.Millisecond * 293 * i % 1000));
			}

			m_reroll = 0;
		}

		public bool CanRoll()
		{
			return m_reroll < Constants.NUM_ROLLS;
		}

		public Scoresheet GetScoresheet()
		{
			return this.m_scoresheet;
		}

		public List<int> GetDiceValues()
		{
			return m_dices.Select(x => x.GetValue()).ToList();
		}

		public void SetDiceValues(List<int> diceValues)
		{
			for (int i = 0; i < diceValues.Count; i++)
			{
				m_dices[i].SetValue(diceValues[i]);
			}
		}

		public int GetEstimatedScore(Enums.Category category)
		{
			return m_scores.ContainsKey(category) ? m_scores[category] : 0;
		}

		public void RollDices(IEnumerable<int> diceIndices)
		{
			if (m_reroll == Constants.NUM_ROLLS)
			{
				return;
			}
			
			foreach (int index in diceIndices)
			{
				m_dices[index].Roll();
			}
			
			CalculateScores();

			m_reroll++;
		}

		public void CalculateScores()
		{
			List<int> diceValues = GetDiceValues();
			
			m_scores[Enums.Category.ONES] = Scoresheet.GetNumeric(diceValues,1);
			m_scores[Enums.Category.TWOS] = Scoresheet.GetNumeric(diceValues,2);
			m_scores[Enums.Category.THREES] = Scoresheet.GetNumeric(diceValues,3);
			m_scores[Enums.Category.FOURS] = Scoresheet.GetNumeric(diceValues,4);
			m_scores[Enums.Category.FIVES] = Scoresheet.GetNumeric(diceValues,5);
			m_scores[Enums.Category.SIXES] = Scoresheet.GetNumeric(diceValues,6);

			m_scores[Enums.Category.CHOICE] = Scoresheet.GetChoice(diceValues);
			m_scores[Enums.Category.FOUR_OF_A_KIND] = Scoresheet.GetFourCard(diceValues);
			m_scores[Enums.Category.FULL_HOUSE] = Scoresheet.GetFullHouse(diceValues);
			m_scores[Enums.Category.SMALL_STRAIGHT] = Scoresheet.GetSmallStraight(diceValues);
			m_scores[Enums.Category.LARGE_STRAIGHT] = Scoresheet.GetLargeStraight(diceValues);
			
			m_scores[Enums.Category.YACHT] = Scoresheet.GetYacht(diceValues);
		}

		public void FillScoreSheet(Enums.Category category)
		{
			var result = m_scoresheet.FillScore(category, m_scores[category]);
			m_reroll = 0;
		}
	}
}