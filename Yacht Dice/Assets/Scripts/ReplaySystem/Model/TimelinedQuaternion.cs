using System;
using UnityEngine;

namespace CQ.MiniGames.ReplaySystem
{
	[Serializable]
	public class TimelinedQuaternion
	{
		public AnimationCurve x;
		public AnimationCurve y;
		public AnimationCurve z;
		public AnimationCurve w;

		public TimelinedQuaternion()
		{
			x = new AnimationCurve();
			y = new AnimationCurve();
			z = new AnimationCurve();
			w = new AnimationCurve();
		}

		public void Add (Quaternion v)
		{
			float time = ReplayManager.Singleton.GetCurrentTime ();
			
			Add(v, time);
		}
		
		public void Add (Quaternion v, float time)
		{
			x.AddKey (time, v.x);
			y.AddKey (time, v.y);
			z.AddKey (time, v.z);
			w.AddKey (time, v.w);
		}

		public Quaternion Get (float _time)
		{
			return new Quaternion (x.Evaluate (_time), y.Evaluate (_time), z.Evaluate (_time), w.Evaluate (_time));
		}
	}
}