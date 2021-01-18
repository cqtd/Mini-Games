using System;
using UnityEngine;

namespace CQ
{
	public class SingletonManager : SingletonMono<SingletonManager>
	{
		static event Action onDomainReload;
		
		public static void Register(Action action)
		{
			var a = Instance;
			onDomainReload += action;
		} 
		
		/// <summary>
		/// 도메인 리로드 없이 static 변수를 초기화합니다.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void ReloadDomain()
		{
			onDomainReload?.Invoke();
			onDomainReload = null;
		}
	}
}