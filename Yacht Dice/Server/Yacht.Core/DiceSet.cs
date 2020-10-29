using System;
using System.Linq;

namespace CQ.MiniGames.Core
{
	public class DiceSet
	{
		public static DiceSet instance { get; private set; }
		public const int DICE_COUNT = 5;
		
		readonly Dice[] dices = new Dice[DICE_COUNT];

		public Action onRoll;
		
		public Dice this[int index] {
			get
			{
				return dices[index];
			}
		}

		public DiceSet(int seed, int interval)
		{
			for (int i = 0; i < DICE_COUNT; i++)
			{
				dices[i] = new Dice(seed + i * interval);
			}

			instance = this;
		}

		public DiceSet() : this(DateTime.Now.Millisecond, (int)(DateTime.Now.Millisecond * 177f % 100))
		{
			
		}

		public void Roll()
		{
			foreach (Dice dice in dices)
			{
				dice.Roll();
			}
			
			onRoll?.Invoke();
		}
		
		#region Score

		public int GetEstimatedScore(EScoreSlot slot)
		{
			switch (slot)
			{
				case EScoreSlot.Acees:
				case EScoreSlot.Duece:
				case EScoreSlot.Three:
				case EScoreSlot.Fours:
				case EScoreSlot.Fives:
				case EScoreSlot.Sixes:
					return GetNumeric((int) slot);
				
				case EScoreSlot.Choic:
					return GetChoice();
				case EScoreSlot.Fourc:
					return GetFourCard();
				case EScoreSlot.FullH:
					return GetFullHouse();
				case EScoreSlot.Small:
					return GetSmallStraight();
				case EScoreSlot.Large:
					return GetLargeStraight();
				case EScoreSlot.Yacht:
					return GetYacht();
				
				default:
					throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
			}
		}
		
		int GetNumeric(int i)
		{
			return dices.Count(e => e.Evaluate() == i) * i;
		}

		int GetChoice()
		{
			return dices.Sum(e => e.Evaluate());
		}

		int GetFourCard()
		{
			for (int i = 1; i <= 6; i++)
			{
				int count = dices.Count(e => e.Evaluate() == i);
				if (count == 5)
				{
					return count * i;
				}
				
				if (count == 4)
				{
					return count * i + dices.First(e => e.Evaluate() != i).Evaluate();
				}
			}

			return 0;
		}

		int GetFullHouse()
		{
			for (int i = 1; i <= 6; i++)
			{
				int count = dices.Count(e => e.Evaluate() == i);
				
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
						
						int count2 = dices.Count(e => e.Evaluate() == j);
						if (count2 >= 2)
						{
							return count * i + count2 * j;
						}
					}
				}
			}

			return 0;
		}

		int GetSmallStraight()
		{
			int[] values = dices.Select(e => e.Evaluate()).ToArray();
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
		
		int GetLargeStraight()
		{
			int[] values = dices.Select(e => e.Evaluate()).ToArray();
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

		int GetYacht()
		{
			for (int i = 1; i <= 6; i++)
			{
				int count = dices.Count(e => e.Evaluate() == i);
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