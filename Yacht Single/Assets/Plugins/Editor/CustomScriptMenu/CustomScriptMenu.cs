using System;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomScriptCreator
{
	internal class CustomScriptMenu
	{
		private static string TemplateDirectory => Application.dataPath + "/Editor/Script Templates/";
		private static string EditorWindowScriptPath => 
#if UNITY_2020_2_OR_NEWER
		                                                TemplateDirectory + "EditorWindow_2020_2_OR_NEWER.cs.txt";
#else
		                                                TemplateDirectory + "EditorWindow_2020_1_OR_LOWER.cs.txt";
#endif
		private static string EditorScriptPath => 
#if UNITY_2020_2_OR_NEWER
		                                                TemplateDirectory + "Editor_2020_2_OR_NEWER.cs.txt";
#else
		                                          TemplateDirectory + "Editor_2020_1_OR_LOWER.cs.txt";
#endif
		private static string SingletonScriptPath => TemplateDirectory + "Singleton.cs.txt"; 
		private static string DynamicSingletonScriptPath => TemplateDirectory + "DynamicSingleton.cs.txt"; 
		private static string StaticSingletonScriptPath => TemplateDirectory + "StaticSingleton.cs.txt"; 

		private const string MENU = "Assets/Create/Predefined Scripts/";
		private const int ORDER = 80;

		[MenuItem(MENU + "Editor Window", false, ORDER)]
		private static void CreateEditorWindowScript()
		{
			RProjectWindowUtil.CreateScriptAssetFromTemplateFile(EditorWindowScriptPath, "NewEditorWindow.cs");
		}

		[MenuItem(MENU + "Editor", false, ORDER + 1)]
		private static void CreateEditorScript()
		{
			string defaultNewFileName = "NewEditor.cs";

			if (EditorScriptPath == null)
			{
				throw new ArgumentNullException(nameof(EditorScriptPath));
			}

			if (!File.Exists(EditorScriptPath))
			{
				throw new FileNotFoundException("The template file \"" + EditorScriptPath + "\" could not be found.",
					EditorScriptPath);
			}

			if (string.IsNullOrEmpty(defaultNewFileName))
			{
				defaultNewFileName = Path.GetFileName(EditorScriptPath);
			}

			Texture2D image = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

			RProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
				ScriptableObject.CreateInstance<DoCreateEditorScriptAsset>(), defaultNewFileName, image,
				EditorScriptPath);
		}

		[MenuItem(MENU + "Singleton", false, ORDER + 20)]
		private static void CreateSingletonScript()
		{
			string defaultNewFileName = "MySingleton.cs";
			
			if (!File.Exists(SingletonScriptPath))
			{
				throw new FileNotFoundException("The template file \"" + SingletonScriptPath + "\" could not be found.",
					SingletonScriptPath);
			}
			
			Texture2D image = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

			RProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
				ScriptableObject.CreateInstance<DoCreateEditorScriptAsset>(), defaultNewFileName, image,
				SingletonScriptPath);
		}
		
		[MenuItem(MENU + "DynamicSingleton", false, ORDER + 21)]
		private static void CreateDynamicSingletonScript()
		{
			string defaultNewFileName = "MyDynamicSingleton.cs";
			
			if (!File.Exists(DynamicSingletonScriptPath))
			{
				throw new FileNotFoundException("The template file \"" + DynamicSingletonScriptPath + "\" could not be found.",
					DynamicSingletonScriptPath);
			}
			
			Texture2D image = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

			RProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
				ScriptableObject.CreateInstance<DoCreateEditorScriptAsset>(), defaultNewFileName, image,
				DynamicSingletonScriptPath);
		}
		
		[MenuItem(MENU + "StaticSingleton", false, ORDER + 22)]
		private static void CreateStaticSingletonScript()
		{
			string defaultNewFileName = "MyStaticSingleton.cs";
			
			if (!File.Exists(StaticSingletonScriptPath))
			{
				throw new FileNotFoundException("The template file \"" + StaticSingletonScriptPath + "\" could not be found.",
					StaticSingletonScriptPath);
			}
			
			Texture2D image = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

			RProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
				ScriptableObject.CreateInstance<DoCreateEditorScriptAsset>(), defaultNewFileName, image,
				StaticSingletonScriptPath);
		}

		public class DoCreateEditorScriptAsset : EndNameEditAction
		{
			public override void Action(int instanceId, string pathName, string resourceFile)
			{
				string resourceContent = File.ReadAllText(resourceFile);
				resourceContent = EditorPreprocessScriptAssetTemplate(pathName, resourceContent);

				string processed
#if UNITY_2020_2_OR_NEWER
					= RProjectWindowUtil.PreprocessScriptAssetTemplate(pathName, resourceContent);
				Object asset = RProjectWindowUtil.CreateScriptAssetWithContent(pathName, processed);
#else
					= resourceContent;
				Object asset = RProjectWindowUtil.CreateScriptAssetFromTemplate(pathName, processed);
#endif
				RProjectWindowUtil.ShowCreatedAsset(asset);
			}
		}

		private static string EditorPreprocessScriptAssetTemplate(string pathName, string resourceContent)
		{
			string withoutExtension = Path.GetFileNameWithoutExtension(pathName);

			string targetClassName = "";

			if (withoutExtension.EndsWith("Inspector", StringComparison.OrdinalIgnoreCase))
			{
				targetClassName = withoutExtension.Replace("Inspector", "");
				targetClassName = targetClassName.Replace("inspector", "");
			}
			else if (withoutExtension.EndsWith("Editor", StringComparison.OrdinalIgnoreCase))
			{
				targetClassName = withoutExtension.Replace("Editor", "");
				targetClassName = targetClassName.Replace("editor", "");
			}

			const string wildcard = "#CUSTOMEDITORATTRIBUTE#";

			if (string.IsNullOrEmpty(targetClassName))
			{
				return resourceContent.Replace(wildcard, "");
			}
			else
			{
				return resourceContent.Replace(wildcard, $"[CustomEditor(typeof({targetClassName}))]");
			}
		}
		
	}
}