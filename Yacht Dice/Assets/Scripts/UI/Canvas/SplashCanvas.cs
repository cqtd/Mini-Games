using System;
using System.Collections;
using CQ.UI;
using DG.Tweening;
using MEC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class SplashCanvas : UICanvas
	{
		public CanvasGroup logoGroup = default;
		public CanvasGroup buttonGroup = default;
		public TextMeshProUGUI progressText = default;
		public DiceLoadingUI loading = default;

		public Button enterButton = default;
		
		[Header("Data")]
		public LoadingContext[] contexts = default;
		
		public float startUpInterval = 1.0f;
		[NonSerialized] public CanvasGroup group = default;

		protected override void InitComponent()
		{
			logoGroup.alpha = 0;
			progressText.SetText("");

			buttonGroup.alpha = 0;
			buttonGroup.interactable = false;
			buttonGroup.blocksRaycasts = false;

			@group = GetComponent<CanvasGroup>();

			enterButton.onClick.AddListener(OpenGameCanvas);
		}

		public override void Initialize()
		{
			base.Initialize();

#if UNITY_EDITOR
			OpenGameCanvas();
			return;
#endif
			
			// 딜레이 콜
			Timing.CallDelayed(startUpInterval, () =>
			{
				// 로고 그룹 켜기
				logoGroup.DOFade(1.0f, 1.0f);
				loading.BeginLoading();
			});

			// 로딩 컨텍스트 업데이트
			StartCoroutine(UpdateLoadingContext());
		}

		public override void Dispose()
		{
			enterButton.onClick.RemoveListener(OpenGameCanvas);
		}

		void OpenGameCanvas()
		{
			LobbyCanvas lobbyCanvas = YachtUIManager.Instance.Open<LobbyCanvas>();
			lobbyCanvas.canvasGroup.alpha = 0;
			
			Tweener tweener = lobbyCanvas.canvasGroup.DOFade(1.0f, Duration.Fast);
			tweener.OnComplete(() =>
			{
				YachtUIManager.Instance.Close<SplashCanvas>();
			});
		}
		
		IEnumerator UpdateLoadingContext()
		{
			yield return new WaitForSeconds(startUpInterval);
			
			// 컨텍스트 로직
			foreach (LoadingContext context in contexts)
			{
				yield return new WaitForSeconds(context.interval);
				progressText.SetText(context.script);
			}
			
			yield return new WaitForSeconds(startUpInterval);
			
			progressText.SetText("로딩 끝...");
			yield return new WaitForSeconds(startUpInterval);
			
			// 로딩 끝
			loading.EndLoading();

			// 로딩 텍스트 페이드 아웃
			progressText.DOFade(0.0f, 1.0f);
			progressText.raycastTarget = false;
			
			yield return new WaitForSeconds(startUpInterval);

			// 버튼 활성화
			var tweener = buttonGroup.DOFade(1.0f, 1.0f);
			tweener.OnComplete(() =>
			{
				buttonGroup.interactable = true;
				buttonGroup.blocksRaycasts = true;
			});
		}
	}
}