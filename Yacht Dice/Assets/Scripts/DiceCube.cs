using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace CQ.MiniGames
{
	using ReplaySystem;
	using Yacht;
	
	[RequireComponent(typeof(Rigidbody))]
	public class DiceCube : MonoBehaviour
	{
		public float threshold = 0.0001f;

		[SerializeField] Rigidbody m_rigidbody = default;
		[SerializeField] Collider m_collider = default;
		[SerializeField] MeshRenderer m_renderer = default;
		[SerializeField] ReplayEntity m_replay = default;
		
		IDisposable positionStream;

		public ReplayEntity GetReplayEntity()
		{
			return m_replay;
		}
		
		public bool IsMoving { get; set; }
		public bool IsSimulating { get; set; }
		public bool IsLocked { get; set; }
		public Vector3 PlacedPosition { get; set; }
		public Quaternion PlacedRotation { get; set; }
		public int DiceValue { get; set; }

		public event Action<bool> onLockStateChanged;
		public event Action onMovementStop;

		void Reset()
		{
			m_rigidbody = GetComponent<Rigidbody>();
			m_collider = GetComponent<Collider>();
			m_renderer = GetComponent<MeshRenderer>();
			m_replay = GetComponent<ReplayEntity>();
		}

		void Start()
		{
			CreateMaterialInstance();
			CreateVelocityStream();
		}

		void CreateMaterialInstance()
		{
			Material mat = Instantiate(m_renderer.sharedMaterial);
			mat.color = Color.black;
			m_renderer.sharedMaterial = mat;
		}

		void CreateVelocityStream()
		{
			positionStream?.Dispose();
			positionStream = transform.UpdateAsObservable()
				.Select(x => m_rigidbody.velocity.sqrMagnitude < threshold && m_rigidbody.angularVelocity.sqrMagnitude < threshold)
				.DistinctUntilChanged()
				.ThrottleFrame(3)
				.Subscribe(OnStateChange);
		}

		void OnStateChange(bool isStopped)
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

		public void SetPosition(Vector3 position)
		{
			transform.position = position;
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

		void ValidateValue()
		{
			m_replay.upside = DiceResolver.GetResult(transform, 1.5f);
			DiceValue = (int) m_replay.upside;
		}

		void OnDrawGizmos()
		{
			if (!Application.isPlaying)
			{
				return;
			}

			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + m_rigidbody.velocity);
		}

		void RefreshColor()
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

		void OnMouseDown()
		{
			Debug.Log("OnMouseDown", this.gameObject);
			IsLocked = !IsLocked;
			
			onLockStateChanged?.Invoke(IsLocked);
			RefreshColor();
		}
	}
}