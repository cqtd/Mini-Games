using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
	static List<Monster> instances;
	public static Action onMonsterExtinct; 

	void Awake()
	{
		if (instances == null)
			instances = new List<Monster>();
	}


	void OnEnable()
	{
		Debug.Log("OnEnable");
		
		instances.Add(this);
	}

	void OnDisable()
	{
		Debug.Log("OnDisable");

		instances.Remove(this);
		
		if (instances.Count < 1)
			onMonsterExtinct.Invoke();
	}
}