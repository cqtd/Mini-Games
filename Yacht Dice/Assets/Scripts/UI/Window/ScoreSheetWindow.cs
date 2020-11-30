using CQ.UI;
using TMPro;
using UnityEngine;

namespace CQ.MiniGames.UI
{
	using Yacht;
	
	public class ScoreSheetWindow : UIWindow
	{
		[SerializeField] ScoreSheetElement[] m_leftSide = default;
		[SerializeField] ScoreSheetElement[] m_rightSide = default;

		[SerializeField] TextMeshProUGUI m_bonusSum = default;
		[SerializeField] TextMeshProUGUI m_bonusEarned = default;
		[SerializeField] TextMeshProUGUI m_sum = default;

		[SerializeField] string bonusFormat = $"{{0}}/{Constants.BONUS_GOAL}";

		public bool CanFill = false;
		public override void InitComponent()
		{
			foreach (ScoreSheetElement element in m_leftSide)
			{
				element.InitComponent();
			}
			
			foreach (ScoreSheetElement element in m_rightSide)
			{
				element.InitComponent();
			}

			m_bonusSum.SetText(string.Format(bonusFormat, 0));
			m_bonusEarned.SetText("0");
			m_sum.SetText("0");
		}

		Player player;

		public void Initialize(Player player)
		{
			this.player = player;
			
			for (int i = 0; i < 6; i++)
			{
				int index = i;
				m_leftSide[i].AddListener(() => Fill(index));
				m_leftSide[i].Activate();
			}
			
			for (int i = 6; i < 12; i++)
			{
				int index = i;
				m_rightSide[i-6].AddListener(() => Fill(index));
				m_rightSide[i-6].Activate();
			}
		}

		public void ActivateAvailableSheet()
		{
			for (int i = 0; i < 6; i++)
			{
				m_leftSide[i].Activate();
				m_leftSide[i].SetPreview(player.GetEstimatedScore((Enums.Category) i));
			}
			
			for (int i = 6; i < 12; i++)
			{
				m_rightSide[i-6].Activate();
				m_rightSide[i-6].SetPreview(player.GetEstimatedScore((Enums.Category) i));
			}

			CanFill = true;
		}
		
		public void DeactivateAvailableSheet()
		{
			for (int i = 0; i < 6; i++)
			{
				m_leftSide[i].Deactivate();
				m_leftSide[i].ClearPreview();
			}
			
			for (int i = 6; i < 12; i++)
			{
				m_rightSide[i-6].Deactivate();
				m_rightSide[i-6].ClearPreview();
			}			
		}

		public void Fill(int category)
		{
			player.FillScoreSheet((Enums.Category) category);
			var score = player.GetScoresheet().GetScore((Enums.Category) category);

			if (category < 6)
			{
				m_leftSide[category].SetValue(score);
				m_leftSide[category].Deactivate();
			}
			else
			{
				m_rightSide[category - 6].SetValue(score);
				m_rightSide[category - 6].Deactivate();
			}
			
			m_bonusSum.SetText(string.Format(bonusFormat, player.GetScoresheet().GetUpperPoint()));
			m_bonusEarned.SetText(player.GetScoresheet().GetBonusPoint().ToString());
			m_sum.SetText(player.GetScoresheet().GetTotalScore().ToString());

			CanFill = false;
		}
	}
}