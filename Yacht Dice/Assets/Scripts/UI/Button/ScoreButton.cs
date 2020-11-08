using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace CQ.MiniGames.UI
{
	public class ScoreButton : Button
	{
		[SerializeField] TextMeshProUGUI textComponent = default;
		
		#if UNITY_EDITOR
		protected override void Reset()
		{
			base.Reset();
			
			Transform child = transform.GetChild(0);
			if (child == null)
			{
				child = new GameObject("TMP Text").transform;
				child.SetParent(this.transform);
			}
			
			textComponent = child.GetComponent<TextMeshProUGUI>();
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
		#endif

		public void Confirm()
		{
			interactable = false;
		}

		public void SetText(string text)
		{
			textComponent.SetText(text);
		}
		
		public void SetText(int value)
		{
			textComponent.SetText(value.ToString());
		}

		public void SetColor(Color color)
		{
			textComponent.color = color;
		}
	}
}