using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Yacht.UIToolkit
{
	[DefaultExecutionOrder(100)]
	[RequireComponent(typeof(UIDocument))]
	public abstract class ScreenBase : MonoBehaviour
	{
		[SerializeField] protected UIDocument document = default;

		protected abstract void OnEnable();
		protected abstract void OnDisable();

		protected virtual void Reset()
		{
			document = GetComponent<UIDocument>();
		}

		public abstract void Show();
		public abstract void Hide();
	}
}