using UnityEngine;

namespace Yacht.ReplaySystem
{
	public class VisualDice : DiceBase
	{
		public Vector3 rotationOffset;
		
		protected override void Reset()
		{
			
		}

		private void OnMouseDown()
		{
			Debug.Log("Visual Dice Clicked", gameObject);
			
			ChangeLockState(!isLocked);
		}

		private void ChangeLockState(bool locked)
		{
			isLocked = locked;
			
			RefreshColor();
		}
	}
}