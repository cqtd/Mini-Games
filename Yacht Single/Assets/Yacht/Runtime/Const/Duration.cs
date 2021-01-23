namespace CQ.MiniGames
{
	/// <summary>
	/// SO 로 컨버팅하기
	/// </summary>
	public class Duration
	{
		public const float GLOBAL_MULTIPLIER = 1.0f;

		public static float Default {
			get
			{
				return 1.0f * GLOBAL_MULTIPLIER;
			}
		}

		public static float Fast {
			get
			{
				return 0.8f * GLOBAL_MULTIPLIER;
			}
		}

		public static float VeryFast {
			get
			{
				return 0.6f * GLOBAL_MULTIPLIER;
			}
		}

		public static float Slow {
			get
			{
				return 1.2f * GLOBAL_MULTIPLIER;
			}
		}

		public static float VerySlow {
			get
			{
				return 1.4f * GLOBAL_MULTIPLIER;
			}
		}

		public const float RAPID = 0.2f;
		public const float FASTEST = 0.4f;
		
		public const float VERYFAST = 0.6f;
		public const float FAST = 0.8f;
		
		public const float DEFAULT = 1.0f;
		
		public const float SLOW = 1.2f;
		public const float VERYSLOW = 1.4f;
		
		public const float SLOWEST = 1.6f;
		public const float SNAIL = 1.8f;
	}
}