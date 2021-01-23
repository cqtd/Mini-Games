using UnityEngine.UIElements;

namespace Yacht.UIToolkit
{
	public class Title : VisualElement
	{
		private VisualElement m_titleScreen = default;
		private VisualElement m_howToScreen = default;
		private VisualElement m_optionScreen = default;
		private VisualElement m_creditScreen = default;

		private const string sceneName = "Yacht";

		public Title()
		{
			this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
		}

		public new class UxmlFactory : UxmlFactory<Title, UxmlTraits>
		{
			
		}
		
		public new class  UxmlTraits : VisualElement.UxmlTraits
		{
			private UxmlStringAttributeDescription m_startScene = new UxmlStringAttributeDescription()
			{
				name = "start-scene", defaultValue = "main",
			};

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);

				var sceneName = m_startScene.GetValueFromBag(bag, cc);

				((Title) ve).Init(sceneName);
			}
		}

		private void OnGeometryChange(GeometryChangedEvent evt)
		{
			m_titleScreen = this.Q("Title");
			m_optionScreen = this.Q("Option");
			m_creditScreen = this.Q("Credit");
			m_howToScreen = this.Q("HowTo");

			m_titleScreen?.Q("start-button").RegisterCallback<ClickEvent>(OnStartGame);
			
			// m_titleScreen?.Q("option_button").RegisterCallback<ClickEvent>(ShowOption);
			// m_titleScreen?.Q("credit_button").RegisterCallback<ClickEvent>(ShowCredit);
			//
			// m_optionScreen?.Q("back_button").RegisterCallback<ClickEvent>(ShowTitle);
			// m_creditScreen?.Q("back_button").RegisterCallback<ClickEvent>(ShowTitle);
			// m_howToScreen?.Q("back_button").RegisterCallback<ClickEvent>(ShowTitle);
			
			
			this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
		}

		public void ShowTitle(ClickEvent clickEvent)
		{
			m_titleScreen.style.display = DisplayStyle.Flex;
			m_optionScreen.style.display = DisplayStyle.None;
			m_creditScreen.style.display = DisplayStyle.None;
		}

		public void ShowOption(ClickEvent clickEvent)
		{
			m_titleScreen.style.display = DisplayStyle.None;
			m_optionScreen.style.display = DisplayStyle.Flex;
			m_creditScreen.style.display = DisplayStyle.None;
		}

		public void ShowCredit(ClickEvent clickEvent)
		{
			m_titleScreen.style.display = DisplayStyle.None;
			m_optionScreen.style.display = DisplayStyle.None;
			m_creditScreen.style.display = DisplayStyle.Flex;
		}

		private void OnStartGame(ClickEvent clickEvent)
		{
			
		}

		public void Init(string sceneName)
		{
			
		}
	}
}