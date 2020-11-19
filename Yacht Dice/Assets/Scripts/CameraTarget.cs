using System;
using UnityEngine;

namespace CQ.MiniGames
{
	public class CameraTarget : MonoBehaviour
	{
		#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position, 0.2f);
			UnityEditor.Handles.Label(transform.position + Vector3.up * 1f, Vector3.Distance(transform.position, Camera.main.transform.position).ToString());
		}
		#endif
	}
}