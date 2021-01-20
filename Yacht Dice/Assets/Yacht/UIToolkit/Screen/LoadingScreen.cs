using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Yacht.UIToolkit
{
	[RequireComponent(typeof(UIDocument))]
	public class LoadingScreen : ScreenBase
	{
		private Label loadingText = default;
		private VisualElement progressElement = default;
		private VisualElement background = default;

		public float fadeSpeed = 1.0f;
		public float progressSpeed = 3.0f;

		private void Awake()
		{
			
		}
		
		protected override void OnEnable()
		{
			loadingText = document.rootVisualElement.Q<Label>("loading-text");
			loadingText.style.display = DisplayStyle.Flex;

			progressElement = document.rootVisualElement.Q<VisualElement>("progress-bar-value");
			progressElement.style.display = DisplayStyle.Flex;
			progressElement.style.width = new StyleLength(new Length(0, LengthUnit.Percent));

			background = document.rootVisualElement.Q<VisualElement>("background");
			
			Patchable.Instance.onLoadingProgressUpdate += SetProgress;

			Engine.onLogging += Print;
		}

		protected override void OnDisable()
		{
			if (Patchable.IsValid)
			{
				Patchable.Instance.onLoadingProgressUpdate -= SetProgress;
			}
			
			Engine.onLogging -= Print;
		}

		public override void Show()
		{
			background.style.display = DisplayStyle.Flex;
		}

		public override void Hide()
		{
			background.style.display = DisplayStyle.None;
		}

		public void Print(string msg)
		{
			loadingText.text = msg;
		}

		private float _progress;

		public void SetProgress(float progress)
		{
			_progress = progress;
		}

		private void Update()
		{
			float value = Mathf.Lerp(progressElement.style.width.value.value, _progress, progressSpeed * Time.deltaTime);
			progressElement.style.width = new StyleLength(new Length(value, LengthUnit.Percent));
		}

		public void FadeIn()
		{
			Engine.Log("LoadingUI::FadeIn");
			StartCoroutine(FadeInCoroutine());
		}

		private IEnumerator FadeInCoroutine()
		{
			Engine.Log("LoadingUI::FadeInCoroutine");
			yield return new WaitForSeconds(1.0f);
			
			float value = 0f;
			while (value < 0.95f)
			{
				value += Time.deltaTime * fadeSpeed;
				background.style.opacity = (1 - value);

				yield return null;
			}

			value = 1.0f;
			background.style.opacity = 0f;
			background.style.display = DisplayStyle.None;
			
			// gameObject.SetActive(false);
			
			Engine.Log("LoadingUI::FadeInCoroutine::Done");
		}
	}
}