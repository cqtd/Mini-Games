using System;

namespace CQ.MiniGames.Core
{
	public class Dice
	{
		readonly Random random;
		
		int value = 0;
		bool locked = false;

		public Action<int> onDiceRolled = default;
		public Action<bool> onDiceLocked = default;
		

		public Dice(int seed)
		{
			random = new Random(seed);
		}

		public Dice()
		{
			random = new Random(DateTime.Now.Millisecond);
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

		public void Roll()
		{
			if (!locked)
			{
				this.SetValue(random.Next(1, 6));
			}
		}

		void SetValue(int newValue)
		{
			this.value = newValue;
			this.onDiceRolled?.Invoke(this.value);
		}

		public void SetEmpty()
		{
			this.SetValue(0);
		}

		public int Evaluate()
		{
			return this.value;
		}
	}
}