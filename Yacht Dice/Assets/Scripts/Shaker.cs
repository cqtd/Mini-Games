using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace CQ.MiniGames
{
	public class Shaker : MonoBehaviour
	{
		IEnumerator Start()
		{
			var rb = GetComponent<Rigidbody>();
			while (true)
			{
				transform.DOShakePosition(5f, 0.4f, 5);
				yield return new WaitForSeconds(5f);
			}
		}
	}
}