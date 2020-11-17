using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace CQ.MiniGames
{
	[RequireComponent(typeof(Rigidbody))]
	public class DiceCube : MonoBehaviour
	{
		Vector3 previousPosition;
		public float threshold = 0.0001f;
		
		public bool IsMoving { get; set; }
		public bool IsSimulating { get; set; }

		IDisposable positionStream;
		
		void Start()
		{
			// var mat = new Material(Shader.Find("Standard"));
			// mat.color = Color.black;
			//
			// GetComponent<MeshRenderer>().sharedMaterial = mat;
		}

		void CreateStream(Action<bool> subscribe)
		{
			positionStream?.Dispose();
			
			previousPosition = transform.position;
			positionStream = transform.UpdateAsObservable()
				.Select(x => Vector3.SqrMagnitude(transform.position - previousPosition) < threshold)
				.Where(x =>
				{
					previousPosition = transform.position;
					return true;
				})
				.DistinctUntilChanged()
				.Throttle(TimeSpan.FromSeconds(1))
				// .ThrottleFrame(5)
				.Subscribe(subscribe);
		}

		void SetState(bool isStopped)
		{
			UnityEngine.Debug.Log("SetState");
			IsMoving = !isStopped;
			GetComponent<MeshRenderer>().sharedMaterial.color = IsMoving ? Color.black : Color.red;
		}

		public void BeginSimulate(Action onCubeStopped)
		{
			CreateStream(isStopped =>
			{
				if (isStopped)
				{
					onCubeStopped?.Invoke();
				}
			});
		}

		public void EndSimulate()
		{
			
		}
	}
}