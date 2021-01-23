using UnityEngine;

internal sealed class DontDestroy : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}