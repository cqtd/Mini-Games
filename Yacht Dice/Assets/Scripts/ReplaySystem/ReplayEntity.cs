using System;
using System.Collections;
using UnityEngine;

namespace CQ.MiniGames.ReplaySystem
{
	public class ReplayEntity : MonoBehaviour
	{
		[NonSerialized] public RecordData data;

		bool m_recording;

		float startTime;

		public void Record()
		{
			data = new RecordData();
			
			m_recording = true;
			startTime = Time.time;
			
			StartCoroutine(RecordingClip());
		}

		public void Abort()
		{
			m_recording = false;
		}

		IEnumerator RecordingClip()
		{
			while (m_recording)
			{
				yield return new WaitForSeconds(1.0f / Application.targetFrameRate);
				data.Add(transform, Time.time - startTime);
			}
		}

		public void Replay (float t)
		{
			data.Set (t, transform);
		}
	}
}