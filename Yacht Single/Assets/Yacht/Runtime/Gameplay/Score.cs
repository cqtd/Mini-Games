﻿using System.Collections.Generic;
using System.Linq;

namespace Yacht.Gameplay
{
	public static class Score
	{
		public static int GetBest(List<int> dices, out Enums.Category bestFit)
		{
			int score = 0;
			
			bestFit = Enums.Category.ONES;
			
			int current = GetYacht(dices);
			if (current >= score)
			{
				bestFit = Enums.Category.YACHT;
				score = current;
			}
			
			current = GetLargeStraight(dices);
			if (current >= score)
			{
				bestFit = Enums.Category.LARGE_STRAIGHT;
				score = current;
			}
			
			current = GetSmallStraight(dices);
			if (current >= score)
			{
				bestFit = Enums.Category.SMALL_STRAIGHT;
				score = current;
			}
			
			current = GetFullHouse(dices);
			if (current >= score)
			{
				bestFit = Enums.Category.FULL_HOUSE;
				score = current;
			}
			
			current = GetFourCard(dices);
			if (current >= score)
			{
				bestFit = Enums.Category.FOUR_OF_A_KIND;
				score = current;
			}
			
			current = GetChoice(dices);
			if (current >= score)
			{
				bestFit = Enums.Category.CHOICE;
				score = current;
			}

			for (int i = 6; i > 0; i--)
			{
				current = GetNumeric(dices, i);
				if (current >= score)
				{
					bestFit = (Enums.Category) (i - 1);
					score = current;
				}
			}

			return score;
		}

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
					var hash = new HashSet<int>(dices);
					if (hash.Count >= 4)
					{
						return 15;
					}
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
					var hash = new HashSet<int>(dices);
					if (hash.Count >= 5)
					{
						return 30;
					}
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
	}
}