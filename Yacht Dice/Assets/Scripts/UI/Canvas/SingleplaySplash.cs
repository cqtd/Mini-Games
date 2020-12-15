using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.MiniGames.UI
{
	public class SingleplaySplash : MonoBehaviour
	{
		public Image background;
		public CanvasGroup logo;

		public TextMeshProUGUI text;

		public Button button;
		public CanvasGroup group;

		private Color color;
		private bool bIsLoadingComplete;
		
		private void Awake()
		{
			group.alpha = 0;
			group.blocksRaycasts = false;
			button.interactable = false;
			text.text = "";
			color = background.color;
			background.color = Color.black;
			logo.alpha = 0;
		}

		private IEnumerator Start()
		{
			Tweener tween = background.DOColor(color, 1.0f);
			while (tween.IsPlaying())
			{
				yield return null;
			} 
			
			tween =  logo.DOFade(1.0f, 1.2f);
			while (tween.IsPlaying())
			{
				yield return null;
			}

			yield return Loading();

			while (!bIsLoadingComplete)
			{
				yield return null;
			}

			yield return new WaitForSeconds(1.0f);

			tween = text.DOFade(0.0f, 0.5f);
			while (tween.IsPlaying())
			{
				yield return null;
			}

			button.interactable = true;
			
			tween = group.DOFade(1.0f, 1.0f);
			while (tween.IsPlaying())
			{
				yield return null;
			}

			group.blocksRaycasts = true;
		}

		private IEnumerator Loading()
		{
			yield return null;
		}
	}
}