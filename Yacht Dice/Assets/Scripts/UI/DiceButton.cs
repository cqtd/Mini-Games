using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	using Core;
	
	public class DiceButton : Toggle
	{
		Dice entity = default;
		bool init = false;

		
		
		public void Init(Dice dice)
		{
			if (init) return;
			init = true;
			
			this.entity = dice;
			
			entity.onDiceRolled += OnDiceRolled;
			entity.onDiceLocked += OnDiceLocked;

			onValueChanged.AddListener(OnValueChanged);
		}

		void OnValueChanged(bool value)
		{
			
			
			entity.Toggle();

		}

		void OnDiceRolled(int obj)
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

		void OnDiceLocked(bool locked)
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