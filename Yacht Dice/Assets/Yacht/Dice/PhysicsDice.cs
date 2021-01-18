using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace CQ.MiniGames
{
	using Yacht.Gameplay.ReplaySystem;
	
	[RequireComponent(typeof(Rigidbody))]
	public class PhysicsDice : DiceBase
	{
		public float threshold = 0.0001f;
		
		[SerializeField] protected Rigidbody m_rigidbody = default;
		[SerializeField] protected Collider m_collider = default;
		[SerializeField] protected ReplayEntity m_replay = default;

		public bool IsMoving { get; set; }
		public bool IsSimulating { get; set; }
		public bool IsLocked { get; set; }
		public Vector3 PlacedPosition { get; set; }
		public Quaternion PlacedRotation { get; set; }
		public int DiceValue { get; set; }

		public event Action<bool> onLockStateChanged;
		public event Action onMovementStop;

		protected override void Start()
		{
			base.Start();
			
			CreateMaterialInstance();
			CreateVelocityStream();
		}

		protected override void Reset()
		{
			base.Reset();
			
			m_rigidbody = GetComponent<Rigidbody>();
			m_collider = GetComponent<Collider>();
			m_replay = GetComponent<ReplayEntity>();
		}

		private void CreateMaterialInstance()
		{
			Material mat = Instantiate(m_renderer.sharedMaterial);
			mat.color = Color.black;
			m_renderer.sharedMaterial = mat;
		}

		private void CreateVelocityStream()
		{
			positionStream?.Dispose();
			positionStream = transform
				.UpdateAsObservable()
				.Select(x =>
					m_rigidbody.velocity.sqrMagnitude < threshold &&
					m_rigidbody.angularVelocity.sqrMagnitude < threshold)
				.DistinctUntilChanged()
				.ThrottleFrame(3)
				.Subscribe(OnStateChange);
		}

		private void OnStateChange(bool isStopped)
		{
			IsMoving = !isStopped;
			RefreshColor();

			if (!IsMoving)
			{
				m_rigidbody.velocity = Vector3.zero;
				m_rigidbody.angularVelocity = Vector3.zero;
				m_rigidbody.isKinematic = true;
				m_rigidbody.useGravity = false;

				PlacedPosition = transform.position;
				PlacedRotation = transform.rotation;
				
				ValidateValue();
				
				m_replay.Abort();
				
				onMovementStop?.Invoke();
			}
		}
		
		public void SetSimulatable(bool enable)
		{
			if (enable)
			{
				m_rigidbody.isKinematic = false;
				m_rigidbody.useGravity = true;
			}
			else
			{
				m_rigidbody.isKinematic = true;
				m_rigidbody.useGravity = false;
			}
		}
		
		public void SetCollidable(bool collidable)
		{
			m_collider.enabled = collidable;
		}

		public void SetVelocity(Vector3 velocity, Vector3 angular)
		{
			m_rigidbody.velocity = velocity;
			m_rigidbody.angularVelocity = angular;
		}

		public void Stop()
		{
			m_rigidbody.velocity = Vector3.zero;
			m_rigidbody.angularVelocity = Vector3.zero;
		}

		public void Replay(float t)
		{
			m_replay.Replay(t);
		}

		private void ValidateValue()
		{
			m_replay.upside = DiceResolver.GetResult(transform, 1.5f);
			DiceValue = (int) m_replay.upside;
		}
		
		public ReplayEntity GetReplayEntity()
		{
			return m_replay;
		}

		private void OnDrawGizmos()
		{
			if (!Application.isPlaying)
			{
				return;
			}

			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + m_rigidbody.velocity);
		}

		private void RefreshColor()
		{
			if (IsLocked)
			{
				m_renderer.sharedMaterial.color = Color.green;
			}
			else
			{
				if (IsMoving)
				{
					m_renderer.sharedMaterial.color = Color.black;
				}
				else
				{
					m_renderer.sharedMaterial.color = Color.red;
				}
			}
		}

		private void OnMouseDown()
		{
			Debug.Log("OnMouseDown", this.gameObject);
			IsLocked = !IsLocked;
			
			onLockStateChanged?.Invoke(IsLocked);
			RefreshColor();
		}
	}
}