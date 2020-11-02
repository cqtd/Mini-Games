using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public float checkInterval = 1.0f;
	public Teleport targetTeleport;
	
	public Monster foundMonster = null;
	
	void Start()
	{
		StartCoroutine(CheckRoutine());
	}

	IEnumerator CheckRoutine()
	{
		TryFoundMonster();
		
		while (foundMonster)
		{
			yield return new WaitForSeconds(checkInterval);
			TryFoundMonster();
		}
		
		// 몬스터 모두 사라짐
		// 텔레포트 활성화
		targetTeleport.OpenPortal();
	}

	void TryFoundMonster()
	{
		foundMonster = FindObjectOfType<Monster>();
	}
}