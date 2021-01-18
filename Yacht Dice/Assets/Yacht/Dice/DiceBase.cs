using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yacht.ReplaySystem
{
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
			transformMap = new Dictionary<string, SerializedTransform>();
		}

		private Dictionary<string, SerializedTransform> transformMap;
		private const string defaultKey = "dk";

		public void CacheTransform(string key = defaultKey)
		{
			transformMap[key] = new SerializedTransform(transform);
		}

		public Vector3 GetCachedPosition(string key = defaultKey)
		{
			if (!transformMap.TryGetValue(key, out SerializedTransform s)) return Vector3.zero;
			return s.position;
		}
		
		public Quaternion GetCachedRotation(string key = defaultKey)
		{
			if (!transformMap.TryGetValue(key, out SerializedTransform s)) return Quaternion.identity;
			return s.rotation;
		}
		
		public Vector3 GetCachedScale(string key = defaultKey)
		{
			if (!transformMap.TryGetValue(key, out SerializedTransform s)) return Vector3.one;
			return s.scale;
		}
	}

	[Serializable]
	public struct SerializedTransform
	{
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 scale;

		public SerializedTransform(Transform transform)
		{
			this.position = transform.position;
			this.rotation = transform.rotation;
			this.scale = transform.localScale;
		}
	}
}