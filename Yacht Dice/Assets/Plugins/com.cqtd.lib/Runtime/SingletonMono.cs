using UnityEngine;

namespace CQ
{
	public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
	{
		/// <summary>
		/// Awake에서 DontDestroyOnLoad에 추가합니다.
		/// </summary>
		[SerializeField] bool optionDontDestroy = true;

		protected static T _inst;

		/// <summary>
		/// 싱글턴 인스턴스 접근
		/// </summary>
		public static T Instance {
			get
			{
				if (_inst == null)
				{
					
					SetInstance(FindObjectOfType(typeof(T)) as T);

					if (_inst == null)
					{
						GameObject container = new GameObject();
						SetInstance(container.AddComponent<T>());
						
						container.name = $"[{_inst.GetType().Name}]";
					}
				}

				return _inst;
			}
		}

		protected static void SetInstance(T instance)
		{
			_inst = instance;
			
#if UNITY_EDITOR
			
			if (instance != null)
				Debug.Log($"[{instance.GetType().Name}] Singleton instance is initialized.");
			
#endif
		}

		/// <summary>
		/// DontDestroyOnLoad를 호출해줌
		/// </summary>
		protected void Awake()
		{
			if (_inst == null)
			{
				_inst = this as T;
				OnAwake();
			}
			else
			{
				Debug.LogWarning("There are more than 2 singletons. later one is being destroyed.");
				Destroy(gameObject);
			}

			if (optionDontDestroy)
			{
				DontDestroyOnLoad(gameObject);
			}
		}

		protected virtual void OnAwake()
		{
			
		}

		#region Editor Fast Enter PlayMode

		protected virtual void OnApplicationQuit()
		{
			Release();
		}

		protected virtual void OnDestroy()
		{
			Release();
		}

		/// <summary>
		/// 도메인 리로드 없이 static 변수를 초기화합니다.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void ReloadDomain()
		{
			Release();
		}

		static void Release()
		{
			_inst = null;
		}

		#endregion
	}
}