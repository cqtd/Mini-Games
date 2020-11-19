using System;
using UnityEngine;

namespace CQ.MiniGames.ReplaySystem
{
	[Serializable]
	public class TimelinedVector3
	{
		public AnimationCurve x;
		public AnimationCurve y;
		public AnimationCurve z;

		public TimelinedVector3()
		{
			x = new AnimationCurve();
			y = new AnimationCurve();
			z = new AnimationCurve();
		}
		
		public void Add (Vector3 v)
		{
			float time = ReplayManager.Singleton.GetCurrentTime ();
			
			Add(v, time);
		}

		public void Add(Vector3 v, float time)
		{
			x.AddKey (time, v.x);
			y.AddKey (time, v.y);
			z.AddKey (time, v.z);
		}

		public Vector3 Get (float _time)
		{
			return new Vector3 (x.Evaluate (_time), y.Evaluate (_time), z.Evaluate (_time));
		}
	}
}