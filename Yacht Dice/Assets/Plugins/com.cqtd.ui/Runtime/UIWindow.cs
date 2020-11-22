using System;

namespace CQ.UI
{
	public abstract class UIWindow : UIElement
	{
		public abstract void InitComponent();
		
		public virtual void Open()
		{
			
		}

		public virtual void Close()
		{
			
		}

		protected virtual void OnEnable()
		{
			
		}

		protected virtual void OnDisable()
		{
			
		}
	}

	public abstract class UIWindow<T> : UIElement<T> where T : UIElement
	{
		
	}
}