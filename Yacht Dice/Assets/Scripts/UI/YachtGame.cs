using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames
{
	using UI;
	using Core;
	
	public class YachtGame : MonoBehaviour
	{
		[SerializeField] DiceButton[] diceButtons = default;
		[SerializeField] Button rollButton = default;
		[SerializeField] TextMeshProUGUI buttonText = default;
		[SerializeField] TextMeshProUGUI chanceText = default;
		
		const string spritePath = "Sprite/Dice_{0}";
		
		DiceSet dices;

		public Score player1;
		
		[NonSerialized] public static Sprite[] sprites; 
		
		public void ConfirmScoreTo(EScoreSlot slot)
		{
			player1.ConfirmTo(slot, dices.GetEstimatedScore(slot));
			OnSelect();
		}

		void Awake()
		{
			sprites = new Sprite[6];
			sprites = Resources.LoadAll<Sprite>("Sprite/Dice");
			
			player1 = new Score();
			dices = new DiceSet();
			
			for (int i = 0; i < DiceSet.DICE_COUNT; i++)
			{
				diceButtons[i].Init(dices[i]);
				dices[i].SetEmpty();
			}
			
			rollButton.onClick.AddListener(StartGame);

			buttonText.SetText("게임 시작");
		}
#if UNITY_EDITOR
		[SerializeField]
#else
		const
#endif
		int maxChanceToRoll = 3;
		int chanceToRoll = -1;

#if UNITY_EDITOR
		[SerializeField]
#else
		const
#endif 
		int maxRound = 12;
		int currentRound = -1;

		void StartGame()
		{
			rollButton.onClick.RemoveAllListeners();
			rollButton.onClick.AddListener(Roll);

			chanceToRoll = maxChanceToRoll;
			currentRound = 1;
			
			rollButton.interactable = true;
			buttonText.SetText("굴리기");
		}

		void Roll()
		{
			dices.Roll();

			chanceToRoll -= 1;
			if (chanceToRoll < 1)
			{
				rollButton.interactable = false;
				buttonText.SetText("점수를 선택하세요.");
			}
			
			chanceText.SetText($"{chanceToRoll}/{maxChanceToRoll} Left");
		}

		void OnSelect()
		{
			for (int i = 0; i < DiceSet.DICE_COUNT; i++)
			{
				dices[i].Unlock();
				dices[i].SetEmpty();
			}
			
			rollButton.interactable = true;
			buttonText.SetText("굴리기");

			currentRound += 1;

			if (currentRound > maxRound)
			{
				rollButton.interactable = false;
				buttonText.SetText("게임 종료");
			}

			chanceToRoll = maxChanceToRoll;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void ResetDomain()
		{
			
		}
	}
}