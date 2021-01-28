﻿/// <summary>
/// Normal Singleton Class
/// </summary>
public abstract class Singleton<T> where T : class, new()
{
	private static T _inst;

	public static T Instance {
		get
		{
			if (_inst == null)
			{
				_inst = new T();
			}

			return _inst;
		}
	}
}