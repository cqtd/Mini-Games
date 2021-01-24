using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomScriptCreator
{
	internal class RProjectWindowUtil
	{
#if UNITY_2020_2_OR_NEWER
		private const string assemblyName =
			"UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
#else
	private const string assemblyName =
			"UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
#endif
	

		#region WRAPPER

		public static void ShowCreatedAsset(Object o)
		{
			UnityEditor.ProjectWindowUtil.ShowCreatedAsset(o);
		}

		public static void CreateScriptAssetFromTemplateFile(string templatePath,
			string defaultNewFileName)
		{
#if UNITY_2018_4
			CreateScriptAsset(templatePath, defaultNewFileName);
#else
			UnityEditor.ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, defaultNewFileName);
#endif
		}

		public static void StartNameEditingIfProjectWindowExists(
			int instanceID,
			UnityEditor.ProjectWindowCallback.EndNameEditAction endAction,
			string pathName,
			Texture2D icon,
			string resourceFile)
		{
			UnityEditor.ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
				instanceID,
				endAction,
				pathName,
				icon,
				resourceFile);
		}
		
		#endregion
		
		#region REFLECTION
		
		internal static UnityEngine.Object CreateScriptAssetFromTemplate(
			string pathName,
			string resourceContent)
		{
			string str1 = resourceContent.Replace("#NOTRIM#", "");
			string withoutExtension = Path.GetFileNameWithoutExtension(pathName);
			string str2 = str1.Replace("#NAME#", withoutExtension);
			string str3 = withoutExtension.Replace(" ", "");
			string str4 = str2.Replace("#SCRIPTNAME#", str3);
			string templateContent;
			if (char.IsUpper(str3, 0))
			{
				string newValue = char.ToLower(str3[0]).ToString() + str3.Substring(1);
				templateContent = str4.Replace("#SCRIPTNAME_LOWER#", newValue);
			}
			else
			{
				string newValue = "my" + char.ToUpper(str3[0]).ToString() + str3.Substring(1);
				templateContent = str4.Replace("#SCRIPTNAME_LOWER#", newValue);
			}
			
			return RProjectWindowUtil.CreateScriptAssetWithContent(pathName, templateContent);
		}

		public static Object CreateScriptAssetWithContent(
			string pathName,
			string templateContent)
		{
			return (UnityEngine.Object) CreateScriptAssetWithContentMethod()
				.Invoke(null, new object[] {pathName, templateContent});
		}

#if UNITY_2018_4
		public static void CreateScriptAsset(string templatePath, string destName)
		{
			CreateScriptAssetMethod()?.Invoke(null, new object[] {templatePath, destName});
		}
#endif

#if UNITY_2020_2_OR_NEWER
		internal static string PreprocessScriptAssetTemplate(string pathName, string resourceContent)
		{
			return (string) PreprocessScriptAssetTemplateMethod()
				.Invoke(null, new object[] {pathName, resourceContent});
		}
#endif

		#endregion

		#region INTERNAL

#if UNITY_2018_4
		private static MethodInfo CreateScriptAssetMethod()
		{
			return ProjectWindowUtilType().GetTypeInfo().GetDeclaredMethod("CreateScriptAsset");
		}
#endif

		private static MethodInfo CreateScriptAssetWithContentMethod()
		{
			return ProjectWindowUtilType().GetTypeInfo().GetDeclaredMethod("CreateScriptAssetWithContent");
		}

#if UNITY_2020_2_OR_NEWER
		private static MethodInfo PreprocessScriptAssetTemplateMethod()
		{
			return ProjectWindowUtilType().GetTypeInfo().GetDeclaredMethod("PreprocessScriptAssetTemplate");
		}
#endif

		private static Type ProjectWindowUtilType()
		{
			return AppDomain.CurrentDomain
				.GetAssemblies()
				.First(e => e.FullName == assemblyName)
				.GetTypes().First(e => e.Name == "ProjectWindowUtil");
		}
		
		#endregion
	}
}