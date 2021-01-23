using System;
using System.Collections.Generic;
using System.Linq;

namespace Yacht.Gameplay
{
	public class Player
	{
		private readonly List<Dice> m_dices;
		private readonly Dictionary<Enums.Category, int> m_scores;

		private readonly Scoresheet m_scoresheet = default;
		private int m_reroll;
		private int m_fillCount;

		public event Action onScoreChange = default;
		public event Action onSessionComplete = default;

		public Player()
		{
			m_dices = new List<Dice>(Constants.NUM_DICES);
			m_scores = new Dictionary<Enums.Category, int>(Constants.NUM_SCORES);
			
			m_scoresheet = new Scoresheet();
			
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				m_dices.Add(new Dice(DateTime.Now.Millisecond * 293 * i % 1000));
			}

			m_reroll = 0;
		}

		public Dice this[int value] {
			get
			{
				return m_dices[value];
			}
		}

		/// <summary>
		/// 리롤 횟수가 남아 있고, 적어도 하나의 홀딩 되지 않은 주사위 존재
		/// </summary>
		/// <returns></returns>
		public bool CanRoll()
		{
			return m_reroll < Constants.NUM_ROLLS && m_dices.Any(e => !e.IsHolding());
		}

		public int GetChance()
		{
			return m_reroll;
		}

		public Scoresheet Scoresheet {
			get => m_scoresheet;
		}

		public List<int> GetDiceValues()
		{
			return m_dices.Select(x => x.GetValue()).ToList();
		}

		public List<Dice> GetDices()
		{
			return m_dices;
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

		public void Roll()
		{
			RollDices(GetNotHoldingDiceIndices());
		}

		public void RollDices(IEnumerable<int> diceIndices)
		{
			if (m_reroll == Constants.NUM_ROLLS)
			{
				return;
			}
			
			foreach (int index in diceIndices)
			{
				if (m_dices[index].IsHolding())
					continue;
				
				m_dices[index].Roll();
			}
			
			CalculateScores();

			m_reroll++;
		}

		/// <summary>
		/// 홀드 상태가 아닌 주사위의 인덱스를 구합니다.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<int> GetNotHoldingDiceIndices()
		{
			var entry = new List<int>();
			for (int i = 0; i < Constants.NUM_DICES; i++)
			{
				if (!m_dices[i].IsHolding())
				{
					entry.Add(i);
				}
			}

			return entry;
		}

		/// <summary>
		/// 예상 점수를 계산합니다
		/// </summary>
		public void CalculateScores()
		{
			List<int> diceValues = GetDiceValues();
			
			m_scores[Enums.Category.ONES] = Score.GetNumeric(diceValues,1);
			m_scores[Enums.Category.TWOS] = Score.GetNumeric(diceValues,2);
			m_scores[Enums.Category.THREES] = Score.GetNumeric(diceValues,3);
			m_scores[Enums.Category.FOURS] = Score.GetNumeric(diceValues,4);
			m_scores[Enums.Category.FIVES] = Score.GetNumeric(diceValues,5);
			m_scores[Enums.Category.SIXES] = Score.GetNumeric(diceValues,6);

			m_scores[Enums.Category.CHOICE] = Score.GetChoice(diceValues);
			m_scores[Enums.Category.FOUR_OF_A_KIND] = Score.GetFourCard(diceValues);
			m_scores[Enums.Category.FULL_HOUSE] = Score.GetFullHouse(diceValues);
			m_scores[Enums.Category.SMALL_STRAIGHT] = Score.GetSmallStraight(diceValues);
			m_scores[Enums.Category.LARGE_STRAIGHT] = Score.GetLargeStraight(diceValues);
			
			m_scores[Enums.Category.YACHT] = Score.GetYacht(diceValues);
		}

		/// <summary>
		/// 점수판에 기입합니다.
		/// </summary>
		/// <param name="category"></param>
		public void FillScoreSheet(Enums.Category category)
		{
			m_scoresheet.FillScore(category, m_scores[category]);
			m_reroll = 0;
			m_fillCount++;

			foreach (Dice dice in m_dices)
			{
				dice.Unhold();
				dice.SetValue(0);
			}
			
			onScoreChange?.Invoke();

			// 세션 종료 확인
			if (m_fillCount == Constants.NUM_SCORES)
			{
				onSessionComplete?.Invoke();
				m_fillCount = 0;
			}
		}
	}
}