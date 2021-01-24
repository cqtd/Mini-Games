using UnityEngine;

public abstract class StaticMonoSingleton<T> : MonoBehaviour where T : StaticMonoSingleton<T>
{
	private static T instance;

	public static T Instance {
		get
		{
			Setup();
			return instance;
		}
	}

	private bool isInitialized = false;

	public static void Setup()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<T>();
			
			if (instance != null)
			{
				DontDestroyOnLoad(instance.gameObject);
			}
		}

		if (!instance.isInitialized)
		{
			instance.Initialize();
			instance.isInitialized = true;
		}

#if UNITY_EDITOR
		if (FindObjectsOfType<T>().Length > 1)
		{
			throw new System.Exception("싱글턴 인스턴스가 1개 이상!");
		}
#endif
	}

	public abstract void Initialize();
	
	protected virtual void Awake()
	{
		Setup();
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