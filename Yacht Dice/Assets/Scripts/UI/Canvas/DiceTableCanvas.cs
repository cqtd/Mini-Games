using System.Collections;
using CQ.MiniGames.Yacht;
using CQ.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class DiceTableCanvas : UICanvas
	{
		[SerializeField] TouchHandlerVisual touchHandler = default;
		[SerializeField] ScoreSheetWindow scoreSheet = default;
		[SerializeField] DiceRollButton m_rollButton = default;
			
		[SerializeField] Button m_button = default;
		TextMeshProUGUI m_text = default;

		[SerializeField] Image m_gauge = default;
		[SerializeField] TextMeshProUGUI m_result = default;
		
		
		protected override void InitComponent()
		{
			touchHandler.InitComponent();
			scoreSheet.InitComponent();
			
			m_button.onClick.AddListener(GameStart);
			m_text = m_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
			
			m_text.SetText("게임 시작");
			m_rollButton.gameObject.SetActive(false);
			m_gauge.gameObject.SetActive(false);
		}

		public override void Dispose()
		{
			
		}

		Player player;

		void GameStart()
		{
			player = new Player();
			player.Initialize();
			
			scoreSheet.Initialize(player);
			
			
			m_button.onClick.RemoveAllListeners();
			m_button.gameObject.SetActive(false);
			
			// m_button.onClick.AddListener(Roll);

			m_gauge.gameObject.SetActive(true);
			
			m_rollButton.gameObject.SetActive(true);
			m_rollButton.onPressStart += OnPressStart;
			m_rollButton.onPressEnd += OnPressEnd;
		}

		Coroutine pingpong;

		void OnPressStart()
		{
			if (pingpong != null)
			{
				StopCoroutine(pingpong);
			}
			
			value = 0f;
			readyToRoll = false;
			pingpong = StartCoroutine(PingPongValue());
		}

		void OnPressEnd()
		{
			Roll();
			m_rollButton.interactable = false;
		}

		bool readyToRoll = false;

		void Roll()
		{
			readyToRoll = true;
		}

		float value = 0f;

		bool isIncremental = true;

		public float multiplier = 1.0f;
		public float corner = 0.05f;

		IEnumerator PingPongValue()
		{
			while (!readyToRoll)
			{
				if (isIncremental)
				{
					if (value < 1 - corner)
					{
						value += Time.deltaTime * multiplier;
					}
					else
					{
						isIncremental = false;
					}	
				}
				else
				{
					if (value > corner)
					{
						value -= Time.deltaTime * multiplier;
					}
					else
					{
						isIncremental = true;
					}
				}

				m_gauge.fillAmount = value;
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