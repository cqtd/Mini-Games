using System;
using CQ.MiniGames.Yacht.ReplaySystem;
using UnityEngine;

namespace CQ.MiniGames
{
	public abstract class DiceBase : MonoBehaviour
	{
		[SerializeField] protected MeshRenderer m_renderer = default;
		[SerializeField] protected ReplayEntity m_replay = default;
		
		protected IDisposable positionStream;
		
		protected virtual void Reset()
		{

			m_renderer = GetComponent<MeshRenderer>();
			m_replay = GetComponent<ReplayEntity>();
		}

		public void SetPosition(Vector3 position)
		{
			transform.position = position;
		}
		
		public ReplayEntity GetReplayEntity()
		{
			return m_replay;
		}

		protected virtual void Start()
		{
			
		}
	}
}