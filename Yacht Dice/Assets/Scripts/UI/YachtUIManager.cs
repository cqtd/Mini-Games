using System;
using CQ.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace CQ.MiniGames.UI
{
	public class YachtUIManager : UIManager<YachtUIManager>
	{
		[Header("초기 설정")]
		[Tooltip("이 캔버스가 최초로 열립니다.")]
		[SerializeField]
		private string initialCanvas = default;
		
		[Header("화면 크기 변경 콜백")]
		[Tooltip("화면 크기 변경 스트림 유지 제한 프레임 수")]
		[Range(1,10)] [SerializeField]
		private int throttleFrameCount = 5;
		[Tooltip("화면 크기 변경 최소 감지 수치")]
		[SerializeField]
		private float tolerance = 0.1f;
		
		
		public Vector2 ScreenSize { get; protected set; } 
		public Vector2 Ratio { get; protected set; }

		private static event Action<Vector2> OnScreenSizeChangeCallback;
		private IDisposable ScreenSizeChangeCallback { get; set; }

		private bool IsScreenSizeChanged
		{
			get
			{
				return Math.Abs(ScreenSize.x - Screen.width) > tolerance ||
				       Math.Abs(ScreenSize.y - Screen.height) > tolerance;
			}
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			
			CreateScreenSizeChangeStream();
			
			Instance.Open(initialCanvas);
		}

		public static void RegisterScreenSizeChange(Action<Vector2> callback)
		{
			if (!IsValid)
				return;

			OnScreenSizeChangeCallback += callback;
			Instance.OnScreenSizeChanged(true);
		}

		public static void UnregisterScreenSizeChange(Action<Vector2> callback)
		{
			if (!IsValid)
				return;
			
			OnScreenSizeChangeCallback -= callback;
		}

		private void CreateScreenSizeChangeStream()
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

		private void OnScreenSizeChanged(bool changed)
		{
			ScreenSize = new Vector2(Screen.width, Screen.height);
			Ratio = new Vector2(scaler.referenceResolution.x / ScreenSize.x, scaler.referenceResolution.y / ScreenSize.y);
			
			OnScreenSizeChangeCallback?.Invoke(ScreenSize);
		}
	}
}