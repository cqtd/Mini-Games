using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	using Core;
	
	public class YachtGame : MonoBehaviour
	{
		[SerializeField] private DiceButton[] diceButtons = default;
		[SerializeField] private Button rollButton = default;
		[SerializeField] private TextMeshProUGUI buttonText = default;
		[SerializeField] private TextMeshProUGUI chanceText = default;


		[NonSerialized] public static Sprite[] sprites = default;
		
		[NonSerialized] private DiceSet dices = default;
		[NonSerialized] public Score player1 = default;

		private const int MAX_CHANCE_TO_ROLL = 3;
		private const int MAX_GAME_ROUND = 12;

		private int chanceToRoll = -1;
		private int currentGameRound = -1;

		private const string SPRITE_PATH = "Sprite/Dice";
		private const string GAME_START = "게임 시작";
		private const string GAME_END = "게임 종료";
		private const string ROLL_DICE = "굴리기";
		private const string PICK_SCORE = "점수를 선택하세요.";
		private const string LEFT_MSG = "{0}/{1} Left";

		private void Awake()
		{
			InitComponent();
			RegisterEvent();
			SetInitialValue();
		}

		private void InitComponent()
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

		private void RegisterEvent()
		{
			rollButton.onClick.AddListener(LaunchGame);
		}

		private void SetInitialValue()
		{
			buttonText.SetText(GAME_START);
		}

		private void LaunchGame()
		{
			rollButton.onClick.RemoveAllListeners();
			rollButton.onClick.AddListener(Roll);

			chanceToRoll = MAX_CHANCE_TO_ROLL;
			currentGameRound = 1;
			
			rollButton.interactable = true;
			buttonText.SetText(ROLL_DICE);
		}

		private void Roll()
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

		private void OnSelect()
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