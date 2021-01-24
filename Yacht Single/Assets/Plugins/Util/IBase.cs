using UnityEngine;

public interface IBase
{
	
}

public static class IBaseExtension
{
	public static bool IsValid(this IBase iBase)
	{
		if (iBase == null)
		{
			return false;
		}

		if (iBase is Object obj)
		{
			return obj;
		}

		return true;
	}
}