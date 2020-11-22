using CQ.UI;
using UnityEngine;

namespace CQ.MiniGames.UI
{
	public class DiceTableCanvas : UICanvas
	{
		[SerializeField] TouchHandlerVisual touchHandler;
		
		protected override void InitComponent()
		{
			touchHandler.InitComponent();
		}

		public override void Dispose()
		{
			
		}
	}
}