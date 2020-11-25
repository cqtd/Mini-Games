using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	using Core;
	
	public class YachtGame : MonoBehaviour
	{
		[SerializeField] DiceButton[] diceButtons = default;
		[SerializeField] Button rollButton = default;
		[SerializeField] TextMeshProUGUI buttonText = default;
		[SerializeField] TextMeshProUGUI chanceText = default;


		[NonSerialized] public static Sprite[] sprites = default;
		
		[NonSerialized] DiceSet dices = default;
		[NonSerialized] public Score player1 = default;
		
		const int MAX_CHANCE_TO_ROLL = 3;
		const int MAX_GAME_ROUND = 12;

		int chanceToRoll = -1;
		int currentGameRound = -1;

		const string SPRITE_PATH = "Sprite/Dice";
		const string GAME_START = "게임 시작";
		const string GAME_END = "게임 종료";
		const string ROLL_DICE = "굴리기";
		const string PICK_SCORE = "점수를 선택하세요.";
		const string LEFT_MSG = "{0}/{1} Left";

		void Awake()
		{
			InitComponent();
			RegisterEvent();
			SetInitialValue();
		}

		void InitComponent()
		{
			sprites = Resources.LoadAll<Sprite>(SPRITE_PATH);
			
			player1 = new Score();
			dices = new DiceSet();
			
			for (int i = 0; i < DiceSet.DICE_COUNT; i++)
			{
				diceButtons[i].Init(dices[i]);
				dices[i].SetEmpty();
			}
		}

		void RegisterEvent()
		{
			rollButton.onClick.AddListener(LaunchGame);
		}

		void SetInitialValue()
		{
			buttonText.SetText(GAME_START);
		}

		void LaunchGame()
		{
			rollButton.onClick.RemoveAllListeners();
			rollButton.onClick.AddListener(Roll);

			chanceToRoll = MAX_CHANCE_TO_ROLL;
			currentGameRound = 1;
			
			rollButton.interactable = true;
			buttonText.SetText(ROLL_DICE);
		}

		void Roll()
		{
			dices.Roll();

			chanceToRoll -= 1;
			if (chanceToRoll < 1)
			{
				rollButton.interactable = false;
				buttonText.SetText(PICK_SCORE);
			}

			chanceText.SetText(string.Format(LEFT_MSG, chanceToRoll, MAX_CHANCE_TO_ROLL));
		}

		void OnSelect()
		{
			for (int i = 0; i < DiceSet.DICE_COUNT; i++)
			{
				dices[i].Unlock();
				dices[i].SetEmpty();
			}
			
			rollButton.interactable = true;
			buttonText.SetText(ROLL_DICE);

			currentGameRound += 1;

			if (currentGameRound > MAX_GAME_ROUND)
			{
				rollButton.interactable = false;
				buttonText.SetText(GAME_END);
			}

			chanceToRoll = MAX_CHANCE_TO_ROLL;
		}
		
		public void Assign(EScoreSlot slot)
		{
			player1.ConfirmTo(slot, dices.GetEstimatedScore(slot));
			OnSelect();
		}
	}
}