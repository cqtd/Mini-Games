using System;

namespace Yacht.Gameplay
{
	public class Dice
	{
		private readonly System.Random random;

		private int m_value;
		bool locked = false;

		public event Action<int> onDiceRolled = default;
		public event Action<bool> onDiceLocked = default;

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

		public bool IsLocked()
		{
			return locked;
		}
		
		public void Toggle()
		{
			if (locked) Unlock();
			else Lock();
		}
		
		public void Lock()
		{
			locked = true;
			
			onDiceLocked?.Invoke(locked);
		}

		public void Unlock()
		{
			locked = false;
			
			onDiceLocked?.Invoke(locked);
		}
	}
}