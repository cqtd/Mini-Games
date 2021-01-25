using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EasyInspectorLocker
{
	internal class EasyInspectorLocker
	{
		private static EditorWindow _mouseOverWindow;

		[MenuItem("Tools/Shortcut/Lock Inspector &D")] // Alt + D
		private static void LockInspector()
		{
			if (_mouseOverWindow == null)
			{
				if (!EditorPrefs.HasKey("LockableInspectorIndex"))
					EditorPrefs.SetInt("LockableInspectorIndex", 0);
				int i = EditorPrefs.GetInt("LockableInspectorIndex");

				Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
				var findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
				_mouseOverWindow = (EditorWindow) findObjectsOfTypeAll[i];
			}

			if (_mouseOverWindow != null && _mouseOverWindow.GetType().Name == "InspectorWindow")
			{
				Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
				PropertyInfo propertyInfo = type.GetProperty("isLocked");
				
				if (propertyInfo != null)
				{
					bool value = (bool) propertyInfo.GetValue(_mouseOverWindow, null);
					propertyInfo.SetValue(_mouseOverWindow, !value, null);
					_mouseOverWindow.Repaint();
				}
			}
		}
		
		[MenuItem("Tools/Shortcut/Toggle Inspector Lock %l")] // Ctrl + L
		private static void ToggleInspectorLock()
		{
			ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
			ActiveEditorTracker.sharedTracker.ForceRebuild();
		}
	}
}

