using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CQ.MiniGames.Editor
{
	[CustomEditor(typeof(Transform), true)][CanEditMultipleObjects]
	public class TransformEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			Transform t = (Transform) target;


			// Replicate the standard transform inspector gui
			EditorGUIUtility.fieldWidth = 0;
			EditorGUIUtility.labelWidth = 0;

			EditorGUI.indentLevel = 0;
			Vector3 position;
			Vector3 eulerAngles;
			Vector3 scale;

			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label("Position", GUILayout.Width(130));

				if (GUILayout.Button("A", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Position " + t.name);
					t.localPosition = Vector3.zero;
				}

				var color = GUI.backgroundColor;
				GUI.backgroundColor = new Color(0.92f, 0.4f, 0.4f);
				
				if (GUILayout.Button("X", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Position X" + t.name);
					var localPos = t.localPosition;
					localPos.x = 0;
					t.localPosition = localPos;
				}

				GUI.backgroundColor = color;
				GUI.backgroundColor = new Color(0.4f, 0.96f, 0.4f);
				
				if (GUILayout.Button("Y", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Position Y " + t.name);
					var localPos = t.localPosition;
					localPos.y = 0;
					t.localPosition = localPos;
				}
				
				GUI.backgroundColor = color;
				GUI.backgroundColor = new Color(0.4f, 0.4f, 0.92f);

				if (GUILayout.Button("Z", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Position Z" + t.name);
					var local = t.localPosition;
					local.z = 0;
					t.localPosition = local;
				}
				GUI.backgroundColor = color;

				GUILayout.Space(20);
				position = EditorGUILayout.Vector3Field("", t.localPosition);
			}

			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label("Rotation", GUILayout.Width(130));

				if (GUILayout.Button("A", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Rotation " + t.name);
					t.localRotation = Quaternion.identity;
				}
				var color = GUI.backgroundColor;
				GUI.backgroundColor = new Color(0.92f, 0.4f, 0.4f);

				if (GUILayout.Button("X", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Rotation X" + t.name);
					var local = t.localRotation.eulerAngles;
					local.x = 0;
					t.localRotation = Quaternion.Euler(local);
				}

				GUI.backgroundColor = color;
				GUI.backgroundColor = new Color(0.4f, 0.96f, 0.4f);
				
				if (GUILayout.Button("Y", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Rotation Y " + t.name);
					var localPos = t.localRotation.eulerAngles;
					localPos.y = 0;
					t.localRotation = Quaternion.Euler(localPos);
				}
				GUI.backgroundColor = color;
				GUI.backgroundColor = new Color(0.4f, 0.4f, 0.92f);
				
				if (GUILayout.Button("Z", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Rotation Z" + t.name);
					var localPos = t.localRotation.eulerAngles;
					localPos.z = 0;
					t.localRotation = Quaternion.Euler(localPos);
				}
				GUI.backgroundColor = color;
				
				GUILayout.Space(20);
				eulerAngles = EditorGUILayout.Vector3Field("", t.localEulerAngles);
			}

			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label("Scale", GUILayout.Width(130));

				if (GUILayout.Button("A", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Scale " + t.name);
					t.localScale = Vector3.one;
				}
				
				var color = GUI.backgroundColor;
				GUI.backgroundColor = new Color(0.92f, 0.4f, 0.4f);

				if (GUILayout.Button("X", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Scale X" + t.name);
					var local = t.localScale;
					local.x = 1;
					t.localScale = local;
				}
				
				GUI.backgroundColor = color;
				GUI.backgroundColor = new Color(0.4f, 0.96f, 0.4f);

				if (GUILayout.Button("Y", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Scale Y " + t.name);
					var localPos = t.localScale;
					localPos.y = 1;
					t.localScale = localPos;
				}
				
				GUI.backgroundColor = color;
				GUI.backgroundColor = new Color(0.4f, 0.4f, 0.92f);

				if (GUILayout.Button("Z", new[] {GUILayout.Width(24)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Scale Z" + t.name);
					var localPos = t.localScale;
					localPos.z = 1;
					t.localScale = localPos;
				}
				
				GUI.backgroundColor = color;

				GUILayout.Space(20);
				scale = EditorGUILayout.Vector3Field("", t.localScale);
			}

			EditorGUIUtility.fieldWidth = 0;
			EditorGUIUtility.labelWidth = 0;

			GUILayout.Space(10);
			using (new EditorGUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Reset Local", new GUILayoutOption[] {GUILayout.Height(36)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset Local Transforms " + t.name);
					t.transform.localPosition = Vector3.zero;
					t.transform.localRotation = Quaternion.identity;
					t.transform.localScale = Vector3.one;
				}

				if (GUILayout.Button("Reset World", new GUILayoutOption[] {GUILayout.Height(36)}))
				{
					Undo.RegisterCompleteObjectUndo(t, "Reset World Transforms " + t.name);
					t.transform.position = Vector3.zero;
					t.transform.rotation = Quaternion.identity;
					t.transform.localScale = Vector3.one;
				}
			}


			if (GUI.changed)
			{
				Undo.RegisterCompleteObjectUndo(t, "Transform Change");
				t.localPosition = FixIfNaN(position);
				t.localEulerAngles = FixIfNaN(eulerAngles);
				t.localScale = FixIfNaN(scale);
			}
		}

		private Vector3 FixIfNaN(Vector3 v)
		{
			if (float.IsNaN(v.x))
			{
				v.x = 0;
			}

			if (float.IsNaN(v.y))
			{
				v.y = 0;
			}

			if (float.IsNaN(v.z))
			{
				v.z = 0;
			}

			return v;
		}
	}
}