using System;
using UnityEngine;

public class Teleport : MonoBehaviour
{
	public GameObject targetObject;
	public GameObject toObject;

	public bool availbale = false;

	void Awake()
	{
		Monster.onMonsterExtinct += OpenPortal;
	}

	public void OpenPortal()
	{
		availbale = true;
	}

	void Update()
	{
		if (targetObject == null) return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (availbale)
			{
				DoTeleport();
			}
			else
			{
				Debug.LogWarning("available 상태가 아닙니다.");
			}
		}
	}

	void DoTeleport()
	{
		targetObject.transform.position = toObject.transform.position;

		availbale = false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			targetObject = other.gameObject;
			Debug.Log("OnTriggerEnter");
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			targetObject = null;
			Debug.Log("OnTriggerExit");
		}
	}
}