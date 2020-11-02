using System;
using System.Collections;
using DG.Tweening;
using MEC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames
{
	[Serializable]
	public struct LoadingContext
	{
		public string script;
		public float interval;
	}
	
	public class Bootstrap : MonoBehaviour
	{
		public CanvasGroup logoGroup = default;
		public CanvasGroup buttonGroup = default;
		public TextMeshProUGUI progressText = default;
		
		[Header("Data")]
		public LoadingContext[] contexts = default;
		
		public float startUpInterval = 1.0f;
		public int targetFramerate = 60;

		void Awake()
		{
			logoGroup.alpha = 0;
			progressText.SetText("");

			buttonGroup.alpha = 0;
			buttonGroup.interactable = false;
			buttonGroup.blocksRaycasts = false;
		}

		void Start()
		{
			Application.targetFrameRate = targetFramerate;
			
			Timing.CallDelayed(startUpInterval, () =>
			{
				logoGroup.DOFade(1.0f, 1.0f);
			});

			StartCoroutine(UpdateLoadingContext());
		}

		IEnumerator UpdateLoadingContext()
		{
			yield return new WaitForSeconds(startUpInterval);
			
			foreach (LoadingContext context in contexts)
			{
				yield return new WaitForSeconds(context.interval);
				progressText.SetText(context.script);
			}
			
			yield return new WaitForSeconds(startUpInterval);
			
			progressText.SetText("로딩 끝...");
			yield return new WaitForSeconds(startUpInterval);

			progressText.DOFade(0.0f, 1.0f);
			progressText.raycastTarget = false;

			var tweener = buttonGroup.DOFade(1.0f, 1.0f);
			tweener.OnComplete(() =>
			{
				buttonGroup.interactable = false;
				buttonGroup.blocksRaycasts = false;
			});
		}
	}
}