using System.Collections.Generic;
using UnityEngine;

namespace CQ.MiniGames
{
	public class Resource : SingletonMono<Resource>
	{
		private Dictionary<string, Object> map;

		protected override void OnAwake()
		{
			base.OnAwake();
			
			map = new Dictionary<string, Object>();
		}

		public static Object Instantiate(string key)
		{
			return Instantiate<Object>(key);
		}

		public static T Instantiate<T>(string key) where T : Object
		{
			return Instantiate<T>(key, null);
		}

		public static T Instantiate<T>(string key, Transform parent) where T : Object
		{
			if (!Instance.map.ContainsKey(key))
			{
				Instance.map[key] = Resources.Load<T>(key);
			}
			
			return Object.Instantiate(Instance.map[key] as T, parent);
		}
	}
}