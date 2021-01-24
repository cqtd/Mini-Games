using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Utility/Dispatcher")]
public class Dispatcher : DynamicMonoSingleton<Dispatcher>
{
	private readonly Queue<Action> queued = new Queue<Action>();

	private static bool isMainThread = true;
	public static bool InMainThread {
		get
		{
			return isMainThread;
		}
	}
	
	public static void BeginAsync()
	{
		isMainThread = false;
	}

	public static void EndAsync()
	{
		isMainThread = true;
	}

	private void Update()
	{
		lock (queued)
		{
			while (queued.Count > 0)
			{
				queued.Dequeue().Invoke();
			}
		}
	}

	public static void Register(Action action)
	{
		Instance.queued.Enqueue(action);
	}

	public static void Register(IEnumerator action)
	{
		Register(() => Instance.StartCoroutine(action));
	}

	public override void Initialize()
	{
		
	}
}