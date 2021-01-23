using System;
using System.Collections;
using UnityEngine;

namespace Yacht.ReplaySystem
{
	public class DiceVisualizer : MonoBehaviour
	{
		public RecordData Data { get; set; }
		public bool IsPlaying { get; protected set; }
		
		public event Action onPlayComplete;

		public void SetCallback(Action action)
		{
			onPlayComplete = action;
		}
		
		public void SetAnimation(RecordData animData)
		{
			Data = animData;
		}
		
		public void Play()
		{
			if (IsPlaying)
			{
				return;
			}
			
			if (Data == null)
			{
				return;
			}

			StartCoroutine(PlayAnimation());
		}

		private IEnumerator PlayAnimation()
		{
			IsPlaying = true;

			// Reference.Use(elapsedTime);
			

			yield return null;
			onPlayComplete?.Invoke();
			IsPlaying = false;
		}
	}
}