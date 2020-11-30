namespace CQ.MiniGames.Yacht
{
	public class Dice
	{
		private readonly System.Random random;

		private int m_value;

		public Dice(int seed)
		{
			random = new System.Random(seed);
		}

		public void Roll()
		{
			m_value = random.Next(1, 6);
		}

		public int GetValue()
		{
			return m_value;
		}

		public void SetValue(int value)
		{
			this.m_value = value;
		}
	}
}