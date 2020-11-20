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
			/// <summary>
			/// 확인되지 않음
			/// </summary>
			UNDEFINED = 0,
			
			/// <summary>
			/// 1 전면
			/// </summary>
			FORWARD = 1,
			/// <summary>
			/// 6 후면
			/// </summary>
			BACKWARD = 6,
			
			/// <summary>
			/// 3 오른쪽
			/// </summary>
			RIGHT = 3,
			/// <summary>
			/// 4 왼쪽
			/// </summary>
			LEFT = 4,
			
			/// <summary>
			/// 2 상단
			/// </summary>
			TOP = 2,
			/// <summary>
			/// 5 하단
			/// </summary>
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

		[Serializable]
		public enum VisualizeMode
		{
			NONE,
			
			PHYSICS,
			BAKED,
		}
	}
}