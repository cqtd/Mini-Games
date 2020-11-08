using System.Collections;
using CQ.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CQ.MiniGames
{
	[RequireComponent(typeof(Image))]
	public class DiceLoadingUI : UIElement
	{
		public Sprite[] sprites = default;
		
		public float interval = 1.2f;
		public float interval2 = 0.4f;
		public float tweenDuration = 2.0f;

		[FormerlySerializedAs("image")] [SerializeField] Image image1 = default;
		[FormerlySerializedAs("swap")] [SerializeField] Image image2 = default;

		bool activated = false;
		IEnumerator loadingCoroutine = default;

		void Reset()
		{
			image1 = GetComponent<Image>();
			image2 = transform.GetChild(0).GetComponent<Image>();
		}

		void Awake()
		{
			image1.sprite = sprites[0];
			image2.sprite = sprites[0];
			
			image1.SetAlpha(0);
			image2.SetAlpha(0);
			
			image1.raycastTarget = false;

			loadingCoroutine = Loading();
		}

		public void BeginLoading()
		{
			image1.DOFade(1.0f, tweenDuration);
			
			activated = true;
			StartCoroutine(loadingCoroutine);
		}

		public void EndLoading()
		{
			StopCoroutine(loadingCoroutine);
			
			// 로딩 로고 페이드 아웃
			image1.DOFade(0.0f, tweenDuration);
			image2.DOFade(0.0f, tweenDuration);

			activated = false;
		}

		IEnumerator Loading()
		{
			while (activated)
			{
				yield return new WaitForSeconds(interval + interval2);

				Vector3 newRot = new Vector3(0, 0, 90 + image1.transform.localRotation.eulerAngles.z);
				Tweener tweener = image1.transform.DOLocalRotate(newRot, interval);

				Sprite curSprite = sprites[Random.Range(0, sprites.Length)];
				image2.sprite = curSprite;
				
				// 이미지 스왑
				image1.DOFade(0.0f, interval * 0.5f).SetEase(Ease.Linear);
				image2.DOFade(1.0f, interval * 0.5f).SetEase(Ease.Linear);
				
				// 끝나면 초기화
				tweener.OnComplete(() =>
				{
					image1.sprite = curSprite;
					image1.SetAlpha(1);
				});
			}
		}
	}
}