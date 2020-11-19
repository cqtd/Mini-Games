using System;
using System.Collections.Generic;
using System.Linq;

namespace CQ.MiniGames.Yacht
{
	public class Player
	{
		readonly List<Dice> m_dices = new List<Dice>(Constants.NUM_DICES);
		readonly Dictionary<Enums.ECategory, int> m_scores = new Dictionary<Enums.ECategory, int>(Constants.NUM_SCORES);

		Scoresheet m_scoresheet;
		int m_reroll;

		public void Initialize()
		{
			m_scoresheet = new Scoresheet();
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				m_dices[i] = new Dice(DateTime.Now.Millisecond * 293 * i % 1000);
			}

			m_reroll = 0;
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
			
			m_scores[Enums.ECategory.ONES] = Scoresheet.GetNumeric(diceValues,1);
			m_scores[Enums.ECategory.TWOS] = Scoresheet.GetNumeric(diceValues,2);
			m_scores[Enums.ECategory.THREES] = Scoresheet.GetNumeric(diceValues,3);
			m_scores[Enums.ECategory.FOURS] = Scoresheet.GetNumeric(diceValues,4);
			m_scores[Enums.ECategory.FIVES] = Scoresheet.GetNumeric(diceValues,5);
			m_scores[Enums.ECategory.SIXES] = Scoresheet.GetNumeric(diceValues,6);

			m_scores[Enums.ECategory.CHOICE] = Scoresheet.GetChoice(diceValues);
			m_scores[Enums.ECategory.FOUR_OF_A_KIND] = Scoresheet.GetFourCard(diceValues);
			m_scores[Enums.ECategory.FULL_HOUSE] = Scoresheet.GetFullHouse(diceValues);
			m_scores[Enums.ECategory.SMALL_STRAIGHT] = Scoresheet.GetSmallStraight(diceValues);
			m_scores[Enums.ECategory.LARGE_STRAIGHT] = Scoresheet.GetLargeStraight(diceValues);
			
			m_scores[Enums.ECategory.YACHT] = Scoresheet.GetYacht(diceValues);
		}

		public void FillScoreSheet(Enums.ECategory category)
		{
			var result = m_scoresheet.FillScore(category, m_scores[category]);
		}
	}
}