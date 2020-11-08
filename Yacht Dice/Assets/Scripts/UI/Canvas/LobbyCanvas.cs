using System;
using CQ.UI;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace CQ.MiniGames
{
	public enum ELobbyState
	{
		NONE = 0,
		
		MAIN,
		GAME_SELECT,
		SLIDE,
		SHOP,
		EVENT,

		COUNT
	}
	
	public class LobbyCanvas : UICanvas
	{
		public CanvasGroup canvasGroup;
		public CanvasGroup blurGroup = default;
		public float blurMax = 0.75f;
		public float blurDuration = 1.0f;

		[NonSerialized] Button blurButton;

		[Header("하위 윈도우")] 
		public TopWindow topWindow = default;
		public GameSelectWindow gameSelectWindow = default;
		public BottomWindow bottomWindow;
		public SlideWindow slideWindow = default;

		[Header("게임 메뉴")]
		public Button gameStartButton = default;
		// public Button slide = default;
		
		[NonSerialized] ELobbyState current = ELobbyState.NONE;
		[NonSerialized] public Action<ELobbyState> onStateChanged = default;
		
		public void EnableBlur()
		{
			blurGroup.transform.SetAsLastSibling();
			var tweener = blurGroup.DOFade(blurMax, Duration.VeryFast);
			
			blurGroup.blocksRaycasts = true;
			blurGroup.interactable = true;
			blurButton.image.raycastTarget = true;
		}

		public void DisalbeBlur()
		{
			var tweener = blurGroup.DOFade(0, Duration.VeryFast);
			tweener.OnComplete(() =>
			{
				blurGroup.blocksRaycasts = false;
				blurGroup.interactable = false;
				blurButton.image.raycastTarget = false;
			});
		}
		
		protected override void InitComponent()
		{
			blurButton = blurGroup.GetComponent<Button>();
			blurButton.onClick.AddListener(OnCancel);
			
			blurGroup.alpha = 0;
			blurButton.image.raycastTarget = false;
			
			gameSelectWindow.InitComponent();
			slideWindow.InitComponent();
			topWindow.InitComponent();
			bottomWindow.InitComponent();
			
			gameStartButton.onClick.AddListener(OnClick_GameStart);
			
			SetState(ELobbyState.MAIN);
			
			// topWindow.slide.onClick.AddListener(OnClick_Slide);
			topWindow.onSlideBegin.AddListener(OnClick_Slide);
		}

		void SetState(ELobbyState state)
		{
			if (this.current != state)
			{
				this.current = state;
				onStateChanged?.Invoke(state);
			}
			else
			{
				Debug.Log("State Stays");
			}
		}

		public override void Dispose()
		{
			
		}

		public void OnClick_GameStart()
		{
			if (current == ELobbyState.GAME_SELECT)
			{
				return;
			}
			
			EnableBlur();
			bottomWindow.transform.SetAsLastSibling();
			gameSelectWindow.transform.SetAsLastSibling();
			gameSelectWindow.Open();

			SetState(ELobbyState.GAME_SELECT);
		}

		void OnClick_Slide()
		{
			if (current == ELobbyState.SLIDE)
			{
				return;
			}
			
			EnableBlur();
			slideWindow.transform.SetAsLastSibling();
			SetState(ELobbyState.SLIDE);
			slideWindow.Open();
		}

		public void OnCancel() 
		{
			switch (current)
			{
				case ELobbyState.NONE:
					break;
				case ELobbyState.MAIN:
					break;
				case ELobbyState.GAME_SELECT:
					DisalbeBlur();
					gameSelectWindow.Close();
					break;
				case ELobbyState.SLIDE:
					DisalbeBlur();
					slideWindow.Close();
					break;
				case ELobbyState.SHOP:
					break;
				case ELobbyState.EVENT:
					break;
				case ELobbyState.COUNT:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			SetState(ELobbyState.MAIN);
		}
	}
}