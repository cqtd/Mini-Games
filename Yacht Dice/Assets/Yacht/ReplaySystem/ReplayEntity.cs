using System;
using System.Collections;
using MEC;
using UnityEngine;

namespace Yacht.Gameplay.ReplaySystem
{
	public class ReplayEntity : MonoBehaviour
	{
		public RecordData data { get; set; }
		[SerializeField] private float recordTimeout = 10f;

		private bool m_recording;
		private float startTime;

		private event Action<bool> onRecordingDone;
		private CoroutineHandle timeOut;
		private Coroutine timeOutHandle;
		
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

		private IEnumerator RecordingClip()
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

		private IEnumerator TimeOut()
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