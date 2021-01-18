using UnityEngine;

namespace CQ.MiniGames
{
	public class VisualDice : DiceBase
	{
		protected override void Reset()
		{
			
		}

		private MeshRenderer GetOrCreateMesh()
		{
			if (!m_renderer)
			{
				if (transform.childCount < 1)
				{
					m_renderer = Resource.Instantiate<MeshRenderer>(Paths.VISUAL_DICE, transform);
				}
				else
				{
					m_renderer = transform.GetChild(0).GetComponent<MeshRenderer>();
				}
			}

			return m_renderer;
		}

		public void Show()
		{
			GetOrCreateMesh().enabled = true;
		}

		public void Hide()
		{
			GetOrCreateMesh().enabled = false;
		}
	}
}