using System;

namespace Yacht.Gameplay
{
	public class Dice
	{
		private readonly System.Random random = default;

		private int m_value = default;
		private bool isholding = false;

		public event Action<int> onDiceRolled = default;
		public event Action<bool> onDiceLocked = default;

		public Dice(int seed)
		{
			random = new System.Random(seed);
		}

		public void Roll(bool withoutCallback = false)
		{
			m_value = random.Next(1, 7);
			
			if (!withoutCallback)
			{
				onDiceRolled?.Invoke(m_value);
			}
		}

		public int GetValue()
		{
			return m_value;
		}

		public void SetValue(int value)
		{
			this.m_value = value;
		}

		public bool IsHolding()
		{
			return isholding;
		}
		
		public void Hold(bool withoutCallback = false)
		{
			isholding = true;

			if (!withoutCallback)
			{
				onDiceLocked?.Invoke(isholding);
			}
		}

		public void Unhold(bool withoutCallback = false)
		{
			isholding = false;
			
			if (!withoutCallback)
			{
				onDiceLocked?.Invoke(isholding);
			}
		}
	}
}