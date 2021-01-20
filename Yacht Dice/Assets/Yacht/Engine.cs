using System;
using UnityEngine;

namespace Yacht
{
	public class Engine : MonoBehaviour
	{
		private static Engine instance;
		
		public static void Init()
		{
			if (instance == null)
			{
				instance = FindObjectOfType<Engine>();
				if (instance == null)
				{
					instance = new GameObject("[Engine]").AddComponent<Engine>();
				}
			}
		}

		public static event Action<string> onLogging;
		public static event Action<string, object> onLoggingObject;
		
		public static void Log(string message)
		{
			if (Dispatcher.InMainThread)
			{
				onLogging?.Invoke(message);
			}
			else
			{
				Dispatcher.Register(() =>
				{
					onLogging?.Invoke(message);
				});
			}
		}

		public static void Log(string message, object obj)
		{
			if (Dispatcher.InMainThread)
			{
				onLoggingObject?.Invoke(message, obj);
			}
			else
			{
				Dispatcher.Register(() =>
				{
					onLoggingObject?.Invoke(message, obj);
				});
			}
		}

		public static void LogError(string message)
		{
			if (Dispatcher.InMainThread)
			{
				onLogging?.Invoke($"<color=orange>{message}</color>");
			}
			else
			{
				Dispatcher.Register(() =>
				{
					onLogging?.Invoke($"<color=orange>{message}</color>");
				});
			}
		}

		public static void LogError(string message, object obj)
		{
			if (Dispatcher.InMainThread)
			{
				onLoggingObject?.Invoke($"<color=orange>{message}</color>", obj);
			}
			else
			{
				Dispatcher.Register(() => { onLoggingObject?.Invoke($"<color=orange>{message}</color>", obj); });
			}
		}
	}
}