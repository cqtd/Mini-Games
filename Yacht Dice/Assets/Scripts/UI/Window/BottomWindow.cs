using CQ.MiniGames.UI;
using CQ.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class BottomWindow : UIWindow
	{
		[SerializeField] private BottomTabButton dashboard = default;
		[SerializeField] private BottomTabButton profile = default;
		[SerializeField] private BottomTabButton leaderboard = default;
		[SerializeField] private BottomTabButton strategy = default;
		
		public override void InitComponent()
		{
			Reference.Use(dashboard);
			Reference.Use(profile);
			Reference.Use(leaderboard);
			Reference.Use(strategy);
		}
	}
}