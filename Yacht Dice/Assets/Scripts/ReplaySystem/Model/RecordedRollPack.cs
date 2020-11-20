using System.Collections.Generic;
using UnityEngine;

namespace CQ.MiniGames.Yacht.ReplaySystem
{
	[CreateAssetMenu(menuName = "Replay/Roll Pack", fileName = "RecordedRollPack", order = 50)]
	public class RecordedRollPack : ScriptableObject
	{
		public List<RecordedRoll> dice1;
		public List<RecordedRoll> dice2;
		public List<RecordedRoll> dice3;
		public List<RecordedRoll> dice4;
		public List<RecordedRoll> dice5;
	}
}