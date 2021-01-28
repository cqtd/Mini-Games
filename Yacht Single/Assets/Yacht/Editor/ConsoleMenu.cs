using System;
using UnityEditor;
using UnityEngine;

namespace Yacht.Editor
{
    public class ConsoleMenu
    {
        [MenuItem("Console/Log")]
        private static void Log()
        {
            Debug.Log("This is a basic log.");
        }

        [MenuItem("Console/Log Warning")]
        private static void LogWarning()
        {
            Debug.LogWarning("You should care about this.");
        }

        [MenuItem("Console/Log Exception")]
        private static void LogError()
        {
            Debug.LogException(new NullReferenceException());
        }
    }
}

namespace EFGame.Editor
{
    public class GameInspector : UnityEditor.Editor
    {
        private SerializedProperty m_sessionId = default;
        private SerializedProperty m_localPlayer = default;
        
        private void OnEnable()
        {
            m_sessionId = serializedObject.FindProperty("m_sessionId");
            m_localPlayer = serializedObject.FindProperty("m_localPlayer");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                EditorGUILayout.PropertyField(m_sessionId);
                EditorGUILayout.PropertyField(m_localPlayer);
            }
        }
    }
}
