using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames
{
	public class YachtGame : MonoBehaviour
	{
		public Button roll;
		Dice dice;
		
		const int diceCount = 5;
		
		void Start()
		{
			Application.targetFrameRate = 60;
			
			dice = new Dice();
		}

		public void Roll()
		{
			dice.RollDice();
		}

		public enum EUpperBoard
		{
			None = 0,
			Aces = 1,
			Duces = 2,
			Threes = 3,
			Fours = 4,
			Fives = 5,
			Sixes = 6,
		}

		public enum ELowerBoard
		{
			Choice,
			
			FourKind,
			FullHouse,
			SmallStraight,
			LargeStraight,
			
			Yacht,			
		}

		public int GetUpperScore(int[] dices, EUpperBoard board)
		{
			int score = 0;
			for (int i = 0; i < diceCount; i++)
			{
				if (dices[i] == (int) board)
				{
					score += (int) board;
				}
			}

			return score;
		}

		public int GetChoiceScore(int[] dices)
		{
			return dices.Sum();
		}

		public int GetFourKindScore(int[] dices)
		{
			if (IsYacht(dices))
			{
				return diceCount * dices[0];
			}
			
			for (int i = 1; i <= 6; i++)
			{
				if (dices.Count(e => e == i) >= 4)
				{
					return (diceCount - 1) * i + dices.FirstOrDefault(e => e != i);
				}
			}

			return 0;
		}

		public int GetFullHouseScore(int[] dices)
		{
			if (IsYacht(dices))
			{
				return diceCount * dices[0];
			}
			
			for (int i = 0; i < 6; i++)
			{
				if (dices.Count(e => e == i) >= 3)
				{
					for (int j = 0; j < diceCount; j++)
					{
						if (i == j) continue;

						if (dices.Count(e => e == j) >= 2)
						{
							return i * 3 + j * 2;
						}
					}
				}
			}

			return 0;
		}

		public int GetSSScore(int[] dices)
		{
			if (IsSmallStraight(dices))
			{
				return 15;
			}
			else
			{
				return 0;
			}
		}

		public int GetLSScore(int[] dices)
		{
			if (IsLargetStraight(dices))
			{
				return 30;
			}
			else
			{
				return 0;
			}
		}

		public int GetYachtScore(int[] dices)
		{
			if (IsYacht(dices))
			{
				return 50;
			}
			else
			{
				return 0;
			}
		}

		bool IsSmallStraight(int[] dices)
		{
			// int[] ordered = dices.ToList().OrderBy(e => e).ToArray();
			int[] ordered = dices.OrderBy((a) => a).ToArray();
			
			if (ordered == new[] {1, 2, 3, 4})
				return true;
			
			if (ordered == new[] {2, 3, 4, 5})
				return true;
			
			if (ordered == new[] {3, 4, 5, 6})
				return true;

			return false;
		}

		bool IsLargetStraight(int[] dices)
		{
			var ordered = dices.ToList().OrderBy(e => e).ToArray();
			
			if (ordered == new[] {1, 2, 3, 4, 5})
				return true;
			if (ordered == new[] {2, 3, 4, 5, 6})
				return true;

			return false;
		}

		bool IsYacht(int[] dices)
		{
			int previous = dices[0];
			for (int i = 1; i < diceCount; i++)
			{
				if (previous != dices[i]) return false;
			}

			return true;
		}
	}

	public class Dice
	{
		readonly System.Random random;
		
		public Dice(int seed)
		{
			random = new System.Random(seed);
			
			RollDice();
		}

		public Dice() : this(DateTime.Now.Millisecond)
		{
			
		}

		int Roll()
		{
			return random.Next(1, 6);
		}

		readonly int[] dices = new int[5];
		readonly bool[] locked = new bool[5];

		void RollDices(int[] lockedIndex = null)
		{
			// keep
			if (lockedIndex != null)
			{
				foreach (int index in lockedIndex)
				{
					locked[index] = true;
				}
			}

			// roll
			for (int i = 0; i < dices.Length; i++)
			{
				if (locked[i])
					continue;

				dices[i] = Roll();
			}
		}

		public void RollDice()
		{
			RollDices();
			
			Debug.Log($"{dices[0]}, {dices[1]}, {dices[2]}, {dices[3]}, {dices[4]}");
		}
	}
}
