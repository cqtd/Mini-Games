using UnityEngine;

namespace CQ.MiniGames
{
	public class Settings : SingletonMono<Settings>
	{
		public static PhysicsSetting Physics {
			get
			{
				return Instance.m_physics;
			}
		}

		[SerializeField] private PhysicsSetting m_physics = default;

		protected override void OnAwake()
		{
			base.OnAwake();
			
			
		}
	}
}