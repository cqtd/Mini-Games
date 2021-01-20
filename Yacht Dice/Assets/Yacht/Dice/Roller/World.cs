using UnityEngine;

namespace CQ.MiniGames
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
		}
		
		public static Transform[] ViewPosition {
			get => instance.viewPosition;
		}
		
		public static Transform[] LockPosition {
			get => instance.lockPosition;
		}

		public static Transform StartPosition {
			get => instance.startPosition;
		}
	}
}