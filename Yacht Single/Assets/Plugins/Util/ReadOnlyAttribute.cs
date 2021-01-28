public enum ReadOnlyOption
{
	DISABLE_ALL = 0,
	EDITABLE_PLAYMODE,
	EDITABLE_EDITMODE,
}

public class ReadOnlyAttribute : UnityEngine.PropertyAttribute
{
	public readonly ReadOnlyOption option;
	
	public ReadOnlyAttribute() : this(ReadOnlyOption.DISABLE_ALL)
	{
		
	}
	
	public ReadOnlyAttribute(in ReadOnlyOption option = ReadOnlyOption.DISABLE_ALL)
	{
		this.option = option;
	}
}
