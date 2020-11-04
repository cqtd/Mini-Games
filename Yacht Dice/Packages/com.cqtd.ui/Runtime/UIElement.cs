using UnityEngine;

namespace CQ.UI
{
	public abstract class UIElement : MonoBehaviour
	{
		public virtual void OnSchemeUpdate()
		{
			
		}

		public virtual void Repaint()
		{
			
		}
	}

	public abstract class UIElement<T> : UIElement where T : Object
	{
		public abstract void Set(T element);
	}
}