using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace CQ.MiniGames
{
	public class Shaker : MonoBehaviour
	{
		bool m_shaking;

		public void StopShaking()
		{
			m_shaking = false;
		}

		public void StartShaking()
		{
			StartCoroutine(Start());
		}
		
		IEnumerator Start()
		{
			m_shaking = true;
			
			while (m_shaking)
			{
				transform.DOShakePosition(5f, 0.4f, 5);
				yield return new WaitForSeconds(5f);
			}

			yield return null;
			Debug.Log("Shaking complete.");
		}
	}
}