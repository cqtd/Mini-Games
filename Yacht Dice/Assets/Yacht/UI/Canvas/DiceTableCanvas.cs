using System;
using System.Collections;
using CQ.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yacht.Gameplay;
using Yacht.ReplaySystem;

namespace CQ.MiniGames.UI
{
	public class DiceTableCanvas : UICanvas
	{
		[SerializeField] private TouchHandlerVisual touchHandler = default;
		[SerializeField] private ScoreSheetWindow scoreSheet = default;
		[SerializeField] private DiceRollButton m_rollButton = default;
			
		[SerializeField] private Button m_button = default;
		private TextMeshProUGUI m_text = default;

		[SerializeField] private Image m_gauge = default;
		[SerializeField] private TextMeshProUGUI m_result = default;

		[NonSerialized] private DiceAnimatior diceAnimator = default;

		[Header("Rolling Gauge Properties")]
		[SerializeField] private float m_multiplier = 1.0f;
		[SerializeField] private float m_corner = 0.05f;
		
		private Player player;
		private Coroutine pingpong;

		private float RollingValue { get; set; } = 0f;
		private bool IsIncremental { get; set; } = true;
		private bool ReadyToRoll { get; set; }
		

		protected override void InitComponent()
		{
			touchHandler.InitComponent();
			scoreSheet.InitComponent();
			
			m_button.onClick.AddListener(GameStart);
			m_text = m_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
			
			m_text.SetText("게임 시작");
			m_rollButton.gameObject.SetActive(false);
			m_gauge.gameObject.SetActive(false);

			diceAnimator = FindObjectOfType<DiceAnimatior>();
			Assert.IsNotNull(diceAnimator);
		}

		public override void Dispose()
		{
			
		}


		private void GameStart()
		{
			player = new Player();
			player.Initialize();
			
			scoreSheet.Initialize(player);
			
			
			m_button.onClick.RemoveAllListeners();
			m_button.gameObject.SetActive(false);
			
			// m_button.onClick.AddListener(Roll);

			m_gauge.gameObject.SetActive(true);
			
			m_rollButton.gameObject.SetActive(true);
			
			// register callback
			m_rollButton.onPressStart.AddListener(OnPressStart);
			m_rollButton.onPressEnd.AddListener(OnPressEnd);
			m_rollButton.onPressCancel.AddListener(OnPressCancel);
		}

		private void OnPressStart(PointerEventData eventData)
		{
			if (pingpong != null)
			{
				StopCoroutine(pingpong);
			}
			
			RollingValue = 0f;
			ReadyToRoll = false;
			pingpong = StartCoroutine(PingPongValue());
		}

		private void OnPressCancel(PointerEventData eventData)
		{
			StopCoroutine(pingpong);
			
			RollingValue = 0f;
			ReadyToRoll = false;
			m_gauge.fillAmount = RollingValue;
		}

		private void OnPressEnd(PointerEventData eventData)
		{
			Roll();
			m_rollButton.interactable = false;
		}


		private void Roll()
		{
			ReadyToRoll = true;
		}

		private IEnumerator PingPongValue()
		{
			while (!ReadyToRoll)
			{
				if (IsIncremental)
				{
					if (RollingValue < 1 - m_corner)
					{
						RollingValue += Time.deltaTime * m_multiplier;
					}
					else
					{
						IsIncremental = false;
					}	
				}
				else
				{
					if (RollingValue > m_corner)
					{
						RollingValue -= Time.deltaTime * m_multiplier;
					}
					else
					{
						IsIncremental = true;
					}
				}

				m_gauge.fillAmount = RollingValue;
				yield return null;
			}
			
			// on roll complete
			player.RollDices(new[] {0, 1, 2, 3, 4});
			var values = player.GetDiceValues();
			
			m_result.SetText($"{values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}, ");
			
			scoreSheet.ActivateAvailableSheet();
			m_rollButton.interactable = true;
			while (scoreSheet.CanFill)
			{
				while (player.CanRoll() && scoreSheet.CanFill)
				{
					yield return null;
				}
				
				m_rollButton.interactable = false;
				yield return null;
			}
			
			scoreSheet.DeactivateAvailableSheet();
			m_rollButton.interactable = true;
		}
	}
}