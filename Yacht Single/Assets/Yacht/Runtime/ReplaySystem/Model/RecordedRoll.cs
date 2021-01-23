using UnityEngine;

namespace Yacht.ReplaySystem
{
	public class RecordedRoll : ScriptableObject
	{
		public RecordData[] datas;
		public float length;

		public RollingAnimation Convert()
		{
			return new RollingAnimation() {datas = datas, length = length};
		}

		public static implicit operator RollingAnimation(RecordedRoll so)
		{
			return so.Convert();
		}
	}
}