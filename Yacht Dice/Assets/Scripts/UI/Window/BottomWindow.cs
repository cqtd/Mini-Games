using CQ.MiniGames.UI;
using CQ.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class BottomWindow : UIWindow
	{
		[SerializeField] BottomTabButton dashboard = default;
		[SerializeField] BottomTabButton profile = default;
		[SerializeField] BottomTabButton leaderboard = default;
		[SerializeField] BottomTabButton strategy = default;
		
		public override void InitComponent()
		{
			Reference.Use(dashboard);
			Reference.Use(profile);
			Reference.Use(leaderboard);
			Reference.Use(strategy);
		}
	}
}