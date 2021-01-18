using System;
using UnityEngine;

namespace Yacht.Gameplay.ReplaySystem
{
	public class RecordedRoll : ScriptableObject
	{
		public RecordData[] datas;
		public float length;

		public RollingAnimation Convert()
		{
			return new RollingAnimation() {datas = datas, length = length};
		}
	}
	
	[Serializable]
	public class RollingAnimation
	{
		public RecordData[] datas;
		public float length;
	}
}