using System;
using UnityEngine;

namespace CQ
{
	sealed class DontDestroy : MonoBehaviour
	{
		void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}
	}
}