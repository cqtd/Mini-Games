using System;
using UnityEngine;

namespace CQ.MiniGames
{
	using Yacht;
	using Yacht.ReplaySystem;
	
	public abstract class DiceBase : MonoBehaviour
	{
		[SerializeField] protected MeshRenderer m_renderer = default;
		
		protected IDisposable positionStream;
		
		protected virtual void Reset()
		{
			m_renderer = GetComponent<MeshRenderer>();
		}

		public void SetPosition(Vector3 position)
		{
			transform.position = position;
		}

		protected virtual void Start()
		{
			
		}
	}
}