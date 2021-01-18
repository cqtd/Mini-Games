using System.Collections.Generic;
using System.Linq;

namespace Yacht.Gameplay
{
	public class Scoresheet
	{
		private readonly Dictionary<Enums.Category, int> m_scores = new Dictionary<Enums.Category, int>(Constants.NUM_SCORES);
	
		public bool IsEmpty(Enums.Category category)
		{
			return !m_scores.ContainsKey(category);
		}
		
		public bool FillScore(Enums.Category category, int score)
		{
			if (!IsEmpty(category))
				return false;
			
			m_scores[category] = score;
			return true;
		}

		public int GetScore(Enums.Category category)
		{
			return m_scores.ContainsKey(category) ? m_scores[category] : -1;
		}

		public int GetTotalScore()
		{
			int total = 0;
			for (int i = 0; i < Constants.NUM_SCORES; i++)
			{
				if (m_scores.ContainsKey((Enums.Category) i))
					total += m_scores[(Enums.Category) i];
			}

			total += GetBonusPoint();
			return total;
		}

		public int GetUpperPoint()
		{
			int bonusScore = 0;
			for (int i = 0; i < 6; i++)
			{
				if (m_scores.ContainsKey((Enums.Category) i))
					bonusScore += m_scores[(Enums.Category) i];
			}

			return bonusScore;
		}

		public int GetBonusPoint()
		{
			int bonusScore = 0;
			for (int i = 0; i < 6; i++)
			{
				if (m_scores.ContainsKey((Enums.Category) i))
					bonusScore += m_scores[(Enums.Category) i];
			}

			return bonusScore >= Constants.BONUS_GOAL ? Constants.BONUS_REWARD : 0;
		}

		#region Score Calculation

		public static int GetNumeric(List<int> dices, int i)
		{
			return dices.Count(e => e == i) * i;
		}
		
		public static int GetChoice(List<int> dices)
		{
			return dices.Sum(e => e);
		}

		public static int GetFourCard(List<int> dices)
		{
			for (int i = 1; i <= 6; i++)
			{
				int count = dices.Count(e => e == i);
				if (count == 5)
				{
					return count * i;
				}
				
				if (count == 4)
				{
					return count * i + dices.First(e => e != i);
				}
			}

			return 0;
		}

		public static int GetFullHouse(List<int> dices)
		{
			for (int i = 1; i <= 6; i++)
			{
				int count = dices.Count(e => e == i);
				
				// 5개
				if (count == 5)
				{
					return count * i;
				}
				
				// 같은 눈금이 3개 이상
				if (count >= 3)
				{
					for (int j = 1; j <= 6; j++)
					{
						// 이미 센 눈금은 스킵
						if (i == j) continue;
						
						int count2 = dices.Count(e => e == j);
						if (count2 >= 2)
						{
							return count * i + count2 * j;
						}
					}
				}
			}

			return 0;
		}

		public static int GetSmallStraight(List<int> dices)
		{
			int[] values = dices.Select(e => e).ToArray();
			int[][] entries = new[] {new[] {1, 2, 3, 4}, new[] {2, 3, 4, 5}, new[] {3, 4, 5, 6}};
			
			foreach (int[] arr in entries)
			{
				bool failed = arr.Any(value => !values.Contains(value));

				if (!failed)
				{
					return 15;
				}
			}

			return 0;
		}
		
		public static int GetLargeStraight(List<int> dices)
		{
			int[] values = dices.Select(e => e).ToArray();
			int[][] entries = new[] {new[] {1, 2, 3, 4, 5}, new[] {2, 3, 4, 5, 6}};
			
			foreach (int[] arr in entries)
			{
				bool failed = arr.Any(value => !values.Contains(value));

				if (!failed)
				{
					return 30;
				}
			}

			return 0;
		}

		public static int GetYacht(List<int> dices)
		{
			for (int i = 1; i <= 6; i++)
			{
				int count = dices.Count(e => e == i);
				if (count >= 5)
				{
					return 50;
				}
			}

			return 0;
		}

		#endregion
	}
}