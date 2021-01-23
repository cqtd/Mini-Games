using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
	[SerializeField] private bool optionDontDestroy = true;

	private static T instance;

	public static T Instance {
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();

				if (instance == null)
				{
					 instance = new GameObject().AddComponent<T>();
					 instance.gameObject.name = $"[{instance.GetType().Name}]";
					 
					 DontDestroyOnLoad(instance.gameObject);
				}
			}

			return instance;
		}
	}

	protected virtual void Awake()
	{
#if UNITY_EDITOR
		if (instance != null && !ReferenceEquals(instance, this))
		{
			throw new System.Exception("새로운 싱글턴 인스턴스가 생성되었음");
		}
#endif
	}

	public static bool IsValid {
		get { return instance != null; }
	}

	#region Editor Fast Enter PlayMode

#if UNITY_EDITOR && UNITY_2019_3_OR_NEWER

	protected virtual void OnApplicationQuit() => Release();
	protected virtual void OnDestroy() => Release();

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ReloadDomain() => Release();

#endif

	private static void Release() => instance = null;

	#endregion
}