﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UnityToolbarExtender
{
    [InitializeOnLoad]
	public class CustomToolbarLeft {

		class SceneData
		{
			public string path;
			public GUIContent popupDisplay;
		}
		
		private static bool _deleteKeys = false;

		private static GUIContent savePassiveBtn;
		private static GUIContent saveActiveBtn;
		private static GUIContent clearPlayerPrefsBtn;
		private static GUIContent reloadSceneBtn;
		private static GUIContent startFromFirstSceneBtn;

		static SceneData[] scenesPopupDisplay;
		static string[] scenesPath;
		static string[] scenesBuildPath;
		static int selectedSceneIndex;
		static int selectedEnterPlayMode;

		static CustomToolbarLeft() {
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
			EditorApplication.playModeStateChanged += LogPlayModeState;

			_deleteKeys = false;

			clearPlayerPrefsBtn = EditorGUIUtility.IconContent("SaveFromPlay");
			clearPlayerPrefsBtn.tooltip = "Clear player prefs";

			savePassiveBtn = EditorGUIUtility.IconContent("SavePassive");
			savePassiveBtn.tooltip = "Enable saving player prefs (currently NOT saving)";

			saveActiveBtn = EditorGUIUtility.IconContent("SaveActive");
			saveActiveBtn.tooltip = "Disable saving player prefs (currently saving)";

			reloadSceneBtn = new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Plugins/Editor/CustomToolbar/Icons/LookDevResetEnv@2x.png", typeof(Texture2D)), "Reload scene");

			startFromFirstSceneBtn = new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Plugins/Editor/CustomToolbar/Icons/LookDevSingle1@2x.png", typeof(Texture2D)), "Start from 1 scene");

			RefreshScenesList();
			EditorSceneManager.sceneOpened += HandleSceneOpened;
		}

		static void OnToolbarGUI() {
			GUILayout.FlexibleSpace();

			DrawEnterPlayModeOption();
			
			GUILayout.Space(10);
			
			DrawSceneDropdown();

			GUILayout.Space(10);

			DrawSavingPrefsButton();
			DrawClearPrefsButton();

			GUILayout.Space(10);

			DrawReloadSceneButton();
			DrawStartFromFirstSceneButton();
		}

		static readonly string[] enterPlayModeOption = new[]
		{
			"Disabled",
			"Reload All",
			"Reload Scene",
			"Reload Domain",
			"FastMode",
		};
		

		static void DrawEnterPlayModeOption()
		{
			if (EditorSettings.enterPlayModeOptionsEnabled)
			{
				EnterPlayModeOptions option = EditorSettings.enterPlayModeOptions;
				selectedEnterPlayMode = (int) option + 1;
			}
			else
			{
				selectedSceneIndex = 0;
			}
			
			selectedEnterPlayMode = EditorGUILayout.Popup(selectedEnterPlayMode, enterPlayModeOption, GUILayout.Width(150f));
			
			if (GUI.changed && 0 <= selectedEnterPlayMode && selectedEnterPlayMode < enterPlayModeOption.Length)
			{
				EditorSettings.enterPlayModeOptionsEnabled = selectedEnterPlayMode != 0;
				EditorSettings.enterPlayModeOptions = (EnterPlayModeOptions) (selectedEnterPlayMode - 1);
			}
		}

		private static void LogPlayModeState(PlayModeStateChange state) {
			if (state == PlayModeStateChange.EnteredEditMode && EditorPrefs.HasKey("LastActiveSceneToolbar")) {
				EditorSceneManager.OpenScene(
					SceneUtility.GetScenePathByBuildIndex(EditorPrefs.GetInt("LastActiveSceneToolbar")));
				EditorPrefs.DeleteKey("LastActiveSceneToolbar");
			}
		}

		private static void DrawSavingPrefsButton() {
			if (_deleteKeys) {
				if (GUILayout.Button(savePassiveBtn, ToolbarStyles.commandButtonStyle)) {
					_deleteKeys = false;
					Debug.Log("Enable saving player prefs");
				}
			}
			else {
				if (GUILayout.Button(saveActiveBtn, ToolbarStyles.commandButtonStyle)) {
					_deleteKeys = true;
					Debug.Log("Disable saving player prefs");
				}
			}

			if (_deleteKeys) {
				PlayerPrefs.DeleteAll();
			}
		}

		private static void DrawClearPrefsButton() {
			if (GUILayout.Button(clearPlayerPrefsBtn, ToolbarStyles.commandButtonStyle)) {
				PlayerPrefs.DeleteAll();
				Debug.Log("Clear Player Prefs");
			}
		}

		private static void DrawReloadSceneButton() {
			EditorGUIUtility.SetIconSize(new Vector2(17, 17));
			if (GUILayout.Button(reloadSceneBtn, ToolbarStyles.commandButtonStyle)) {
				if (EditorApplication.isPlaying) {
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}
		}

		private static void DrawStartFromFirstSceneButton() {
			if (GUILayout.Button(startFromFirstSceneBtn, ToolbarStyles.commandButtonStyle)) {
				if (!EditorApplication.isPlaying) {
					EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
					EditorPrefs.SetInt("LastActiveSceneToolbar", EditorSceneManager.GetActiveScene().buildIndex);
					EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
				}

				EditorApplication.isPlaying = !EditorApplication.isPlaying;
			}
		}

		private static void DrawSceneDropdown() {
			selectedSceneIndex = EditorGUILayout.Popup(selectedSceneIndex, scenesPopupDisplay.Select(e => e.popupDisplay).ToArray(), GUILayout.Width(150f));

			if (GUI.changed && 0 <= selectedSceneIndex && selectedSceneIndex < scenesPopupDisplay.Length) {
				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
					foreach (var scenePath in scenesPath) {
						if((scenePath) == scenesPopupDisplay[selectedSceneIndex].path) {
							EditorSceneManager.OpenScene(scenePath);
							break;
						}
					}
				}
			}

		}

		static void RefreshScenesList() {
			
			
			List<SceneData> toDisplay = new List<SceneData>();

			selectedSceneIndex = -1;
			
			scenesBuildPath = EditorBuildSettings.scenes.Select(s => s.path).ToArray();

			string[] sceneGuids = AssetDatabase.FindAssets("t:scene");
			scenesPath = new string[sceneGuids.Length];
			for (int i = 0; i < scenesPath.Length; ++i) {
				scenesPath[i] = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);
			}

			Scene activeScene = SceneManager.GetActiveScene();
			int usedIds = scenesBuildPath.Length;

			for (int i = 0; i < scenesBuildPath.Length; ++i) {
				string name = GetSceneName(scenesBuildPath[i]);
				
				if (selectedSceneIndex == -1 && GetSceneName(name) == activeScene.name)
					selectedSceneIndex = i;

				GUIContent content = new GUIContent(name, EditorGUIUtility.Load("BuildSettings.Editor.Small") as Texture, "Open scene");

				toDisplay.Add(new SceneData()
				{
					path = scenesBuildPath[i],
					popupDisplay = content,
				});
			}

			toDisplay.Add(new SceneData()
			{
				path = "\0",
				popupDisplay = new GUIContent("\0"),
			});
			++usedIds;

			for (int i = 0; i < scenesPath.Length; ++i) {
				if (scenesBuildPath.Contains(scenesPath[i]))
					continue;

				string name = GetSceneName(scenesPath[i]);
				
				if (selectedSceneIndex == -1 && name == activeScene.name)
					selectedSceneIndex = usedIds;

				GUIContent content = new GUIContent(name, "Open scene");

				toDisplay.Add(new SceneData()
				{
					path = scenesPath[i],
					popupDisplay = content,
				});

				++usedIds;
			}

			scenesPopupDisplay = toDisplay.ToArray();
		}

		static void HandleSceneOpened(Scene scene, OpenSceneMode mode) {
			RefreshScenesList();
		}

		static string GetSceneName(string path) {
			path = path.Replace(".unity", "");

			int lastSlash = path.LastIndexOf('/');
			if (0 <= lastSlash && lastSlash <= path.Length)
				path = path.Substring(lastSlash + 1);

			return path;
		}
	}
}
