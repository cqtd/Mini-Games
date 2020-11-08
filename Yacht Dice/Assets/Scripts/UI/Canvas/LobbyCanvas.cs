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

		[Header("슬라이드 메뉴")]
		public SlideWindow slideWindow = default;
		
		[Header("게임 메뉴")]
		public Button gameStartButton = default;
		public GameSelectWindow gameSelectWindow = default;

		[Header("하단 메뉴")] 
		public BottomWindow bottomWindow;
		public Button dashboard = default;
		public Button profile = default;
		public Button leaderboard = default;
		public Button strategy = default;

		[Header("상단 메뉴")] 
		public Button shop = default;
		public Button events = default;
		public Button notice = default;
		
		public Button slide = default;
		
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
			
			gameStartButton.onClick.AddListener(OnClick_GameStart);
			
			SetState(ELobbyState.MAIN);
			
			dashboard.onClick.AddListener(OnClick_Dashboard);
			profile.onClick.AddListener(OnClick_Profile);
			leaderboard.onClick.AddListener(OnClick_Leadeboard);
			strategy.onClick.AddListener(OnClick_Strategy);
			
			shop.onClick.AddListener(OnClick_Shop);
			events.onClick.AddListener(OnClick_Events);
			notice.onClick.AddListener(OnClick_Notice);
			
			slide.onClick.AddListener(OnClick_Slide);
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

		public void OnClick_Dashboard()
		{
			
		}

		public void OnClick_Profile()
		{
			
		}

		public void OnClick_Leadeboard()
		{
			
		}

		public void OnClick_Strategy()
		{
			
		}

		public void OnClick_Shop()
		{
			
		}

		public void OnClick_Events()
		{
			
		}

		public void OnClick_Notice()
		{
			
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