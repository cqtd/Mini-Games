using System.Collections.Generic;
using UnityEngine;

namespace Yacht.ReplaySystem
{
	[CreateAssetMenu(menuName = "Replay/Roll Pack", fileName = "RecordedRollPack", order = 50)]
	public class RecordedRollPack : ScriptableObject
	{
		public List<RecordedRoll> dice1;
		public List<RecordedRoll> dice2;
		public List<RecordedRoll> dice3;
		public List<RecordedRoll> dice4;
		public List<RecordedRoll> dice5;

		public int Count {
			get
			{
				return dice1.Count + dice2.Count + dice3.Count + dice4.Count + dice5.Count;
			}
		}
	}
}