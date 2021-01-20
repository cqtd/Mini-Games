using System;
using UnityEngine;
using Yacht.AssetManagement;
using Object = UnityEngine.Object;

namespace Yacht.ReplaySystem
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
					MeshRenderer resource = Resources.Load<MeshRenderer>(AssetPath.VISUAL_DICE);
					m_renderer = Object.Instantiate<MeshRenderer>(resource, transform);
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

		private void OnMouseDown()
		{
			Engine.Log("Visual Dice Clicked", gameObject);
		}
	}
}