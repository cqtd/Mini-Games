using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		bool disabled = ((ReadOnlyAttribute) attribute).option switch
		{
			ReadOnlyOption.DISABLE_ALL => true,
			ReadOnlyOption.EDITABLE_PLAYMODE => !Application.isPlaying,
			ReadOnlyOption.EDITABLE_EDITMODE => Application.isPlaying,
			_ => true
		};

		using EditorGUI.DisabledScope scope = new EditorGUI.DisabledScope(disabled);
		EditorGUI.PropertyField(position, property, label, true);
	}
}
