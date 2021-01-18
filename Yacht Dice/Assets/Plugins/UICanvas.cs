using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CQ.UI
{
	public abstract class UICanvas<T> : UICanvas where T : MonoBehaviour
	{
		public T Entity { get; private set; } = default;

		public virtual void Initialize(T entity)
		{
			this.Entity = entity;
		}
	}
	
	[RequireComponent(typeof(Canvas))]
	public abstract class UICanvas : MonoBehaviour, IDisposable
	{
		[Header("UI Canvas")]
		[SerializeField] protected Canvas canvas = default;

		bool bIsStartInit = false;
		bool bIsInitComponent = false;
		bool bIsInitialized = false;

		[NonSerialized] public readonly UnityEvent openEvent = new UnityEvent();
		[NonSerialized] public readonly UnityEvent closeEvent = new UnityEvent();

		void InitComponentInternal()
		{
			if (!bIsInitComponent)
			{
				InitComponent();
				bIsInitComponent = true;
			}
		}

		void InitializeInternal()
		{
			if (!bIsInitialized)
			{
				Initialize();
			}
		}

		protected abstract void InitComponent();

		// 최초 Initialize : Start
		protected virtual void Start()
		{
			InitializeInternal();
			bIsStartInit = true;
		}

		// SetActive, False는 OnEnable
		protected virtual void OnEnable()
		{
			if (bIsStartInit)
			{
				InitializeInternal();
			}
		}

		public virtual void Close()
		{
			Dispose();
			gameObject.SetActive(false);
			
			closeEvent?.Invoke();
		}

		public virtual void Initialize()
		{
			InitComponentInternal();	
			bIsInitialized = true;
			
			openEvent?.Invoke();
		}

		public int GetSortingOrder()
		{
			return canvas.sortingOrder;
		}

		public void SetCanvasConfig(bool overrideSorting, int sortingOrder)
		{
			canvas.overrideSorting = overrideSorting;
			canvas.sortingOrder = sortingOrder;
		}

		public void ResetName(string type)
		{
			gameObject.name = $"[{GetSortingOrder():000}] {type}";
		}


#if UNITY_EDITOR
		void Reset()
		{
			canvas = GetComponent<Canvas>();
			
			UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
#endif
		public abstract void Dispose();
	}


}