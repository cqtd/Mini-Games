using UnityEngine;

namespace Yacht
{
	public class World : MonoBehaviour
	{
		public Transform[] viewPosition = default;
		public Transform[] lockPosition = default;

		public Transform startPosition = default;

		private static World instance = default;
		
		public static void Init()
		{
			instance = FindObjectOfType<World>();
			instance.DisableColliders();
		}
		
		public static Transform[] ViewPosition {
			get => instance.viewPosition;
		}
		
		public static Transform[] HoldPosition {
			get => instance.lockPosition;
		}

		public static Transform StartPosition {
			get => instance.startPosition;
		}

		private void DisableColliders()
		{
			var colliders = GetComponentsInChildren<Collider>();
			foreach (Collider col in colliders)
			{
				col.enabled = false;
			}
		}
		
#if UNITY_EDITOR
		private Mesh mesh;
		
		private void OnDrawGizmos()
		{
			if (mesh == null)
			{
				var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
				go.hideFlags = HideFlags.HideAndDontSave;
				mesh = go.GetComponent<MeshFilter>().sharedMesh;
				DestroyImmediate(go);
			}
			
			Gizmos.color = Color.green;
			foreach (Transform view in viewPosition)
			{
				Gizmos.DrawWireMesh(mesh, view.position, Quaternion.Euler(new Vector3(0, 135, 308)), Vector3.one);
			}
			
			Gizmos.color = Color.cyan;
			foreach (Transform view in lockPosition)
			{
				Gizmos.DrawWireMesh(mesh, view.position,  Quaternion.Euler(new Vector3(0, 135, 308)), Vector3.one);
			}
		}
#endif


	}
}