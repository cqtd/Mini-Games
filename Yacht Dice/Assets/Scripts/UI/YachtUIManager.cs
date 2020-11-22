using System;
using CQ.UI;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace CQ.MiniGames.UI
{
	public class YachtUIManager : UIManager<YachtUIManager>
	{
		static event Action<Vector2> OnScreenSizeChangeCallback;
		
		public SplashCanvas Splash { get; protected set; }
		public LobbyCanvas Lobby { get; protected set; }
		
		IDisposable ScreenSizeChangeCallback { get; set; }
		
		const float tolerance = 0.1f;
		
		public Vector2 screenSize { get; protected set; } 
		public Vector2 ratio { get; protected set; }
		
		public int throttleFrameCount = 5;
		
		bool IsScreenSizeChanged
		{
			get
			{
				return Math.Abs(screenSize.x - Screen.width) > tolerance ||
				       Math.Abs(screenSize.y - Screen.height) > tolerance;
			}
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			
			CreateScreenSizeChangeStream();

			SplashCanvas splashCanvas = Open<SplashCanvas>();
			splashCanvas.enterButton.onClick.AddListener(OpenGameMenu);
		}

		public void OpenGameMenu()
		{
			Lobby = Open<LobbyCanvas>();
			Lobby.canvasGroup.alpha = 0;

			Tweener tweener = Lobby.canvasGroup.DOFade(1.0f, Duration.Fast);
			tweener.OnComplete(() =>
			{
				// Splash.gameObject.SetActive(false);
				Close<SplashCanvas>();
			});
		}

		public static void RegisterScreenSizeChange(Action<Vector2> callback)
		{
			OnScreenSizeChangeCallback += callback;

			if (_inst)
			{
				_inst.OnScreenSizeChanged(true);
			}
		}

		public static void UnregisterScreenSizeChange(Action<Vector2> callback)
		{
			OnScreenSizeChangeCallback -= callback;
		}

		void CreateScreenSizeChangeStream()
		{
			if (ScreenSizeChangeCallback == null)
			{
				ScreenSizeChangeCallback = this
					.FixedUpdateAsObservable()
					.Select(_ => IsScreenSizeChanged)
					.DistinctUntilChanged()
					.ThrottleFrame(throttleFrameCount)
					.Where(x => x)
					.Subscribe(OnScreenSizeChanged)
					.AddTo(this);
			}
		}

		void OnScreenSizeChanged(bool changed)
		{
			screenSize = new Vector2(Screen.width, Screen.height);
			ratio = new Vector2(scaler.referenceResolution.x / screenSize.x, scaler.referenceResolution.y / screenSize.y);
			
			OnScreenSizeChangeCallback?.Invoke(screenSize);
		}
	}
}