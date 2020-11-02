using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

namespace CQ.MiniGames
{
	[RequireComponent(typeof(Image))]
	public class DiceLoadingUI : MonoBehaviour
	{
		public Sprite[] sprites = default;
		public float interval = 1.2f;
		public float interval2 = 0.4f;
		public float tweenDuration = 2.0f;

		IEnumerator loadingCoroutine = default;
		[SerializeField] Image image = default;

		bool activated = false;
		int currentIndex = 0;
		
		void Reset()
		{
			image = GetComponent<Image>();
		}

		void Awake()
		{
			image.sprite = sprites[0];
			
			var col = Color.white;
			col.a = 0;
			image.color = col;
			
			// image.enabled = false;
			image.raycastTarget = false;

			loadingCoroutine = Loading();
		}

		public void BeginLoading()
		{
			image.DOFade(1.0f, tweenDuration);
			// image.enabled = true;
			
			activated = true;
			StartCoroutine(loadingCoroutine);
		}

		public void EndLoading()
		{
			StopCoroutine(loadingCoroutine);
			var tweener = image.DOFade(0.0f, tweenDuration);

			tweener.OnComplete(() =>
			{
				// image.enabled = false;
			});

			activated = false;
		}

		IEnumerator Loading()
		{
			while (activated)
			{
				yield return new WaitForSeconds(interval + interval2);
				var tweener =
					image.transform.DOLocalRotate(new Vector3(0, 0, 90 + image.transform.localRotation.eulerAngles.z),
						interval);
				tweener.OnComplete(() =>
				{
					image.sprite = sprites[Random.Range(0, sprites.Length)];
				});
			}
		}
	}
}