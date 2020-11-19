using System;

namespace CQ.MiniGames.Yacht
{
	public class Enums
	{
		public enum Category
		{
			ONES,
			TWOS,
			THREES,
			FOURS,
			FIVES,
			SIXES,
			
			CHOICE,
			FOUR_OF_A_KIND,
			FULL_HOUSE,
			SMALL_STRAIGHT,
			LARGE_STRAIGHT,
			
			YACHT,
		}
		
		[Serializable]
		public enum DiceFace
		{
			UNDEFINED = 0,
			
			FORWARD = 1,
			BACKWARD = 6,
			
			RIGHT = 3,
			LEFT = 4,
			
			TOP = 2,
			BOTTOM = 5,
		}

		[Serializable]
		public enum DiceValue
		{
			NONE = 0,
		
			ONE = 1,
			TWO = 2,
			THREE = 3,
			FOUR = 4,
			FIVE = 5,
			SIX = 6,
		}
	}
}