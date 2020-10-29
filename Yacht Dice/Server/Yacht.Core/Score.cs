using System;
using System.Collections.Generic;

namespace CQ.MiniGames.Core
{
	public class Score
	{
		readonly Dictionary<EScoreSlot, int> scoreMap;

		public Score()
		{
			scoreMap = new Dictionary<EScoreSlot, int>();
		}
		
		public int SubTotal {
			get
			{
				int total = 0;
				
				foreach (EScoreSlot slot in Enum.GetValues(typeof(EScoreNumeric)))
				{
					if (scoreMap.TryGetValue(slot, out int score))
					{
						total += score;
					}
				}

				return total;
			}
		}

		public int Total {
			get
			{
				int total = 0;
				
				foreach (EScoreSlot slot in Enum.GetValues(typeof(EScoreSlot)))
				{
					if (scoreMap.TryGetValue(slot, out int score))
					{
						total += score;
					}
				}

				return total;
			}
		}

		public void ConfirmTo(EScoreSlot slot, int score)
		{
			scoreMap[slot] = score;
		}

		public bool HasScore(EScoreSlot slot)
		{
			return scoreMap.ContainsKey(slot);
		}

		public bool HasScore(EScoreSlot slot, out int score)
		{
			if (HasScore(slot))
			{
				score = scoreMap[slot];
				return true;
			}

			score = 0;
			return false;
		}

		public void Clear()
		{
			scoreMap.Clear();
		}
	}
}