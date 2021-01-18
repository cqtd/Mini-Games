using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	using Core;
	
	public class DiceButton : Toggle
	{
		private Dice entity = default;
		private bool init = false;

		
		
		public void Init(Dice dice)
		{
			if (init) return;
			init = true;
			
			this.entity = dice;
			
			entity.onDiceRolled += OnDiceRolled;
			entity.onDiceLocked += OnDiceLocked;

			onValueChanged.AddListener(OnValueChanged);
		}

		private void OnValueChanged(bool value)
		{
			
			
			entity.Toggle();

		}

		private void OnDiceRolled(int obj)
		{
			if (obj == 0)
			{
				image.sprite = null;
			}
			else
			{
				image.sprite = YachtGame.sprites[obj - 1];
			}
		}

		private void OnDiceLocked(bool locked)
		{
			if (locked == isOn)
			{
				
			}
			else
			{
				SetIsOnWithoutNotify(locked);
				// isOn = locked;
			}
		}
	}
}