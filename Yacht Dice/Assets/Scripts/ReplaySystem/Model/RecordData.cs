using System;
using UnityEngine;

namespace CQ.MiniGames.Yacht.ReplaySystem
{
	[Serializable]
	public class RecordData
	{
		public TimelinedVector3 position;
		public TimelinedQuaternion rotation;
		public TimelinedVector3 scale;
		
		public Enums.DiceFace upside;

		public RecordData()
		{
			position = new TimelinedVector3();
			rotation = new TimelinedQuaternion();
			scale = new TimelinedVector3();
		}

		public void Add(Transform t)
		{
			position.Add(t.position);
			rotation.Add(t.rotation);
			scale.Add(t.localScale);
		}

		public void Add(Transform t, float time)
		{
			position.Add(t.position, time);
			rotation.Add(t.rotation, time);
			scale.Add(t.localScale, time);
		}

		public void Set(float _time, Transform _transform)
		{
			_transform.position = position.Get(_time);
			_transform.rotation = rotation.Get(_time);
			_transform.localScale = scale.Get(_time);
		}
	}
}