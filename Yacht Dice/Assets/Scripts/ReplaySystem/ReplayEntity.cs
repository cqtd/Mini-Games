using System;
using System.Collections;
using CQ.MiniGames.Yacht;
using MEC;
using UnityEngine;

namespace CQ.MiniGames.ReplaySystem
{
	public class ReplayEntity : MonoBehaviour
	{
		public RecordData data { get; set; }
		[SerializeField] float recordTimeout = 10f;

		bool m_recording;
		float startTime;

		event Action<bool> onRecordingDone;
		CoroutineHandle timeOut;
		Coroutine timeOutHandle;
		
		public Enums.DiceFace upside { get; set; }

		public void Record(Action<bool> onComplete)
		{
			onRecordingDone = onComplete;
			
			Record();
		}

		public void Record()
		{
			data = new RecordData();
			
			m_recording = true;
			startTime = Time.time;
			
			StartCoroutine(RecordingClip());
			timeOutHandle = StartCoroutine(TimeOut());
		}

		public void Abort()
		{
			m_recording = false;

			if (timeOutHandle != null)
				StopCoroutine(timeOutHandle);
		}
		
		public void Replay (float t)
		{
			data.Set (t, transform);
		}
		
		IEnumerator RecordingClip()
		{
			while (m_recording)
			{
				yield return new WaitForSeconds(1.0f / Application.targetFrameRate);
				data.Add(transform, Time.time - startTime);
			}

			StopCoroutine(timeOutHandle);

			data.upside = this.upside;
			onRecordingDone?.Invoke(true);
		}

		IEnumerator TimeOut()
		{
			yield return new WaitForSeconds(recordTimeout);
			
			if (m_recording)
			{
				m_recording = false;
				onRecordingDone?.Invoke(false);
			}
		}
	}
}