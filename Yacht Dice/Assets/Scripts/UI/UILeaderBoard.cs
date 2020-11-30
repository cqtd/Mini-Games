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
		[SerializeField] private YachtGame game = default;
		
		[SerializeField] private TextMeshProUGUI playerName = default;
		
		[SerializeField] private ScoreButton aceScore = default;
		[SerializeField] private ScoreButton dueceScore = default;
		[SerializeField] private ScoreButton threeScore = default;
		[SerializeField] private ScoreButton fourScore = default;
		[SerializeField] private ScoreButton fiveScore = default;
		[SerializeField] private ScoreButton sixScore = default;
		
		[SerializeField] private TextMeshProUGUI subTotalScore = default;
		[SerializeField] private TextMeshProUGUI bonusScore = default;
		
		[SerializeField] private ScoreButton choiceScore = default;
		
		[SerializeField] private ScoreButton fourCardScore = default;
		[SerializeField] private ScoreButton fullHouseScore = default;
		[SerializeField] private ScoreButton SmallStrScore = default;
		[SerializeField] private ScoreButton LargeStrScore = default;
		
		[SerializeField] private ScoreButton YachtScore = default;
		
		[SerializeField] private TextMeshProUGUI totalScore = default;

		public Color confirmedTextColor;
		public Color previewTextColor;

		private Dictionary<EScoreSlot, ScoreButton> buttonMap;

		public virtual void Start()
		{
			DiceSet.instance.onRoll += PreviewScore;
			playerName.SetText(game.name);
			
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

			// 클릭 이벤트 바인딩
			foreach (EScoreSlot slot in Enum.GetValues(typeof(EScoreSlot)))
			{
				buttonMap[slot].onClick.AddListener(() =>
				{
					game.Assign(slot);
					
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
		private void Repaint()
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
		private void PreviewScore()
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
		private static void ResetDomain() {
			
		}
	}
}
