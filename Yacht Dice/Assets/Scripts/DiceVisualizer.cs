using System;
using System.Collections;
using CQ.MiniGames.Yacht.ReplaySystem;
using UnityEngine;

namespace CQ.MiniGames
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

		IEnumerator PlayAnimation()
		{
			IsPlaying = true;
			float elapsedTime = 0f;

			Reference.Use(elapsedTime);
			

			yield return null;
			onPlayComplete?.Invoke();
			IsPlaying = false;
		}
	}
}