using System.Collections.Generic;

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

		public int this[Enums.Category category] {
			get
			{
				return GetScore(category);
			}
		}

		private int GetScore(Enums.Category category)
		{
			return m_scores.ContainsKey(category) ? m_scores[category] : -1;
		}

		/// <summary>
		/// 전체 점수
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// 숫자 점수 합계
		/// </summary>
		/// <returns></returns>
		public int GetUpperPoint()
		{
			int sum = 0;
			for (int i = 0; i < 6; i++)
			{
				if (m_scores.ContainsKey((Enums.Category) i))
					sum += m_scores[(Enums.Category) i];
			}

			return sum;
		}

		/// <summary>
		/// 보너스 점수
		/// </summary>
		/// <returns></returns>
		public int GetBonusPoint()
		{
			int bonusScore = GetUpperPoint();

			return bonusScore >= Constants.BONUS_GOAL ? Constants.BONUS_REWARD : 0;
		}

		/// <summary>
		/// 새 스코어 시트
		/// </summary>
		public void Clear()
		{
			m_scores.Clear();
		}
	}
}