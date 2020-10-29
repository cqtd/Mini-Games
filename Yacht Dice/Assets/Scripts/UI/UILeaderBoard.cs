using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CQ.MiniGames.UI
{
	using Core;
	
	[DefaultExecutionOrder(1000)]
	public class UILeaderBoard : MonoBehaviour
	{
		[SerializeField] YachtGame game = default;
		
		[SerializeField] TextMeshProUGUI playerName = default;
		
		[SerializeField] ScoreButton aceScore = default;
		[SerializeField] ScoreButton dueceScore = default;
		[SerializeField] ScoreButton threeScore = default;
		[SerializeField] ScoreButton fourScore = default;
		[SerializeField] ScoreButton fiveScore = default;
		[SerializeField] ScoreButton sixScore = default;
		
		[SerializeField] TextMeshProUGUI subTotalScore = default;
		[SerializeField] TextMeshProUGUI bonusScore = default;
		
		[SerializeField] ScoreButton choiceScore = default;
		
		[SerializeField] ScoreButton fourCardScore = default;
		[SerializeField] ScoreButton fullHouseScore = default;
		[SerializeField] ScoreButton SmallStrScore = default;
		[SerializeField] ScoreButton LargeStrScore = default;
		
		[SerializeField] ScoreButton YachtScore = default;
		
		[SerializeField] TextMeshProUGUI totalScore = default;

		public Color confirmedTextColor;
		public Color previewTextColor;
		
		Dictionary<EScoreSlot, ScoreButton> buttonMap;

		public virtual void Start()
		{
			DiceSet.instance.onRoll += PreviewScore;
			
			buttonMap = new Dictionary<EScoreSlot, ScoreButton>
			{
				[EScoreSlot.Acees] = aceScore,
				[EScoreSlot.Duece] = dueceScore,
				[EScoreSlot.Three] = threeScore,
				[EScoreSlot.Fours] = fourScore,
				[EScoreSlot.Fives] = fiveScore,
				[EScoreSlot.Sixes] = sixScore,
				[EScoreSlot.Choic] = choiceScore,
				[EScoreSlot.Fourc] = fourCardScore,
				[EScoreSlot.FullH] = fullHouseScore,
				[EScoreSlot.Small] = SmallStrScore,
				[EScoreSlot.Large] = LargeStrScore,
				[EScoreSlot.Yacht] = YachtScore
			};
			
			Repaint();

			// // 텍스트 비우기
			// foreach (EScoreSlot type in Enum.GetValues(typeof(EScoreSlot)))
			// {
			// 	buttonMap[type].SetText("");
			// }
			//
			// subTotalScore.SetText($"{game.player1.SubTotal}/63");
			// bonusScore.SetText(game.player1.SubTotal >= 63 ? "35" : "");
			// totalScore.SetText(game.player1.Total.ToString());

			// 클릭 이벤트 바인딩
			foreach (EScoreSlot slot in Enum.GetValues(typeof(EScoreSlot)))
			{
				buttonMap[slot].onClick.AddListener(() =>
				{
					game.ConfirmScoreTo(slot);
					
					buttonMap[slot].interactable = false;
					buttonMap[slot].SetColor(confirmedTextColor);
					
					Repaint();
					EventSystem.current.SetSelectedGameObject(null);
				});
			}
		}

		/// <summary>
		/// 기입한 점수표 그리기
		/// </summary>
		void Repaint()
		{
			foreach (EScoreSlot type in Enum.GetValues(typeof(EScoreSlot)))
			{
				if (game.player1.HasScore(type, out int score))
				{
					buttonMap[type].SetText(score);
				}
				else
				{
					buttonMap[type].SetText("");
				}
			}
			
			subTotalScore.SetText($"{game.player1.SubTotal}/63");
			bonusScore.SetText(game.player1.SubTotal >= 63 ? "35" : "");
			totalScore.SetText(game.player1.Total.ToString());
		}

		/// <summary>
		/// 현재 주사위가 어떤 점수를 주는지
		/// </summary>
		void PreviewScore()
		{
			foreach (EScoreSlot type in Enum.GetValues(typeof(EScoreSlot)))
			{
				if (game.player1.HasScore(type))
					continue;
				
				buttonMap[type].SetText(DiceSet.instance.GetEstimatedScore(type));
				buttonMap[type].SetColor(previewTextColor);
			}
		}
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void ResetDomain() {
			
		}
	}
}
