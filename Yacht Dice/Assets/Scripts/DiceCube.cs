using System;
using CQ.MiniGames.Yacht;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace CQ.MiniGames
{
	using ReplaySystem;
	
	[RequireComponent(typeof(Rigidbody))]
	public class DiceCube : MonoBehaviour
	{
		public float threshold = 0.0001f;
		public LayerMask groundLayer = default;
		
		[SerializeField] Rigidbody m_rigidbody;
		[SerializeField] Collider m_collider;
		[SerializeField] MeshRenderer m_renderer;
		[SerializeField] public ReplayEntity m_replay;
		
		IDisposable positionStream;
		
		public bool IsMoving { get; set; }
		public bool IsSimulating { get; set; }
		public bool IsLocked { get; set; }
		public Vector3 PlacedPosition { get; set; }
		public Quaternion PlacedRotation { get; set; }
		public int DiceValue { get; set; }

		public event Action<bool> onLockStateChanged;
		public event Action onMovementStop;

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
				// m_collider.enabled = true;
			}
			else
			{
				m_rigidbody.isKinematic = true;
				m_rigidbody.useGravity = false;
				// m_collider.enabled = false;
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
			if (Physics.Raycast(transform.position, transform.up, 1.5f, groundLayer.value))
			{
				DiceValue = 5;
				m_replay.upside = Enums.DiceFace.BOTTOM;
			}
			else if (Physics.Raycast(transform.position, -transform.up, 1.5f, groundLayer.value))
			{
				DiceValue = 2;
				m_replay.upside = Enums.DiceFace.TOP;
			}
			else if (Physics.Raycast(transform.position, transform.forward, 1.5f, groundLayer.value))
			{
				DiceValue = 6;
				m_replay.upside = Enums.DiceFace.BACKWARD;
			}
			else if (Physics.Raycast(transform.position, -transform.forward, 1.5f, groundLayer.value))
			{
				DiceValue = 1;
				m_replay.upside = Enums.DiceFace.FORWARD;
			}
			else if (Physics.Raycast(transform.position, transform.right, 1.5f, groundLayer.value))
			{
				DiceValue = 4;
				m_replay.upside = Enums.DiceFace.LEFT;
			}
			else if (Physics.Raycast(transform.position, -transform.right, 1.5f, groundLayer.value))
			{
				DiceValue = 3;
				m_replay.upside = Enums.DiceFace.RIGHT;
			}
			else
			{
				m_replay.upside = Enums.DiceFace.UNDEFINED;
			}
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