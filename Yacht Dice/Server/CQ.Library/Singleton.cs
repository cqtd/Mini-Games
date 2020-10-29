using System;

namespace CQ
{
	public abstract class Singleton<T> : IDisposable where T : class, new()
	{
		internal static T instance { get; private set; } = default;

		public static T Instance {
			get
			{
				if (instance == null)
				{
					instance = new T();
				}

				return instance;
			}
		}

		public void Dispose()
		{
			instance = null;
		}
	}
}