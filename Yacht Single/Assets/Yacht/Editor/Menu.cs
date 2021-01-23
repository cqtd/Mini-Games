using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yacht.AssetManagement;
using Yacht.ReplaySystem;

namespace CQ.MiniGames.Editor
{
	public class Menu
	{
		[InitializeOnLoadMethod]
		public static void Init()
		{
			EditorApplication.playModeStateChanged += LogPlayModeState;
		}

		private static void LogPlayModeState(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.EnteredEditMode && EditorPrefs.HasKey("LastActiveSceneToolbar"))
			{
				EditorSceneManager.OpenScene(
					SceneUtility.GetScenePathByBuildIndex(EditorPrefs.GetInt("LastActiveSceneToolbar")));
				EditorPrefs.DeleteKey("LastActiveSceneToolbar");
			}
		}

		[MenuItem("Tools/Play At First #&P", false, 999)]
		private static void PlayAtFirst()
		{
			if (!EditorApplication.isPlaying)
			{
				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
				EditorPrefs.SetInt("LastActiveSceneToolbar", SceneManager.GetActiveScene().buildIndex);
				EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
			}

			EditorApplication.isPlaying = !EditorApplication.isPlaying;
		}

		#region Replay System

		[MenuItem("Tools/Replay System/Export Baked Animations", false, 10)]
		private static void ExportBakedAnimation()
		{
			EditorUtility.DisplayProgressBar("Replay System - Export", "Preparing processes...", 0f);
			
			RecordedRollPack pack = AssetDatabase.LoadAssetAtPath<RecordedRollPack>("Assets/Animations/RecordedRollPack.asset");
			
			string dir = Application.streamingAssetsPath + Constant.PATCHABLE;
			var dirInfo = new DirectoryInfo(dir);
			
			if (!dirInfo.Exists) dirInfo.Create();

			Dictionary<int, List<string>> map = new Dictionary<int, List<string>>();

			int totalCount = pack.Count;
			int current = 1;
			
			List<string> hashes = new List<string>();
			foreach (RollingAnimation rollData in pack.dice1)
			{
				string hash = GZipCompressEditor.Hasing("dice.animation", $"{rollData.length}_1");
				string serialzied = JsonConvert.SerializeObject(rollData, Formatting.None);
				serialzied = GZipCompress.XORCipher(serialzied, Constant.TEJAVA);

				File.WriteAllBytes(dir + $"/{hash}.{Constant.DICE_ANIM_EXTENSION}", GZipCompressEditor.Zip(serialzied));

				hashes.Add(hash);

				current++;
				EditorUtility.DisplayProgressBar("Replay System - Export", $"pack data 1 [{current:00}/{totalCount}]",
					((float) current / totalCount));
			}

			map[1] = hashes;
			hashes = new List<string>();
			
			foreach (RollingAnimation rollData in pack.dice2)
			{
				string hash = GZipCompressEditor.Hasing("dice.animation", $"{rollData.length}_2");
				string serialzied = JsonConvert.SerializeObject(rollData, Formatting.None);
				serialzied = GZipCompress.XORCipher(serialzied, Constant.TEJAVA);

				File.WriteAllBytes(dir + $"/{hash}.{Constant.DICE_ANIM_EXTENSION}", GZipCompressEditor.Zip(serialzied));
					
				hashes.Add(hash);
				
				current++;
				EditorUtility.DisplayProgressBar("Replay System - Export", $"pack data 2 [{current:00}/{totalCount}]",
					((float) current / totalCount));
			}

			map[2] = hashes;
			hashes = new List<string>();
			
			foreach (RollingAnimation rollData in pack.dice3)
			{
				string hash = GZipCompressEditor.Hasing("dice.animation", $"{rollData.length}_3");
				string serialzied = JsonConvert.SerializeObject(rollData, Formatting.None);
				serialzied = GZipCompress.XORCipher(serialzied, Constant.TEJAVA);
				
				File.WriteAllBytes(dir + $"/{hash}.{Constant.DICE_ANIM_EXTENSION}", GZipCompressEditor.Zip(serialzied));
					
				hashes.Add(hash);
				
				current++;
				EditorUtility.DisplayProgressBar("Replay System - Export", $"pack data 3 [{current:00}/{totalCount}]",
					((float) current / totalCount));
			}

			map[3] = hashes;
			hashes = new List<string>();
			
			foreach (RollingAnimation rollData in pack.dice4)
			{
				string hash = GZipCompressEditor.Hasing("dice.animation", $"{rollData.length}_4");
				string serialzied = JsonConvert.SerializeObject(rollData, Formatting.None);
				serialzied = GZipCompress.XORCipher(serialzied, Constant.TEJAVA);

				File.WriteAllBytes(dir + $"/{hash}.{Constant.DICE_ANIM_EXTENSION}", GZipCompressEditor.Zip(serialzied));
				
				hashes.Add(hash);
				
				current++;
				EditorUtility.DisplayProgressBar("Replay System - Export", $"pack data 4 [{current:00}/{totalCount}]",
					((float) current / totalCount));
			}

			map[4] = hashes;
			hashes = new List<string>();
			
			foreach (RollingAnimation rollData in pack.dice5)
			{
				string hash = GZipCompressEditor.Hasing("dice.animation", $"{rollData.length}_5");
				string serialzied = JsonConvert.SerializeObject(rollData, Formatting.None);
				serialzied = GZipCompress.XORCipher(serialzied, "tejava");

				File.WriteAllBytes(dir + $"/{hash}.{Constant.DICE_ANIM_EXTENSION}", GZipCompressEditor.Zip(serialzied));
					
				hashes.Add(hash);
				
				current++;
				EditorUtility.DisplayProgressBar("Replay System - Export", $"pack data 5 [{current:00}/{totalCount}]",
					((float) current / totalCount));
			}

			map[5] = hashes;
			
			string serializedMap = JsonConvert.SerializeObject(map, Formatting.None);
			serializedMap = GZipCompress.XORCipher(serializedMap, "tejava");
			
			File.WriteAllBytes(dir + $"/hash.bin", GZipCompressEditor.Zip(serializedMap));
			EditorUtility.ClearProgressBar();
		}

		[MenuItem("Tools/Replay System/Import Baked Animations", false, 12)]
		private static void ImportBakedAnimation()
		{
			string dir = Application.streamingAssetsPath + Constant.PATCHABLE;
			
			byte[] bytes = File.ReadAllBytes(dir + $"/hash.bin");
			string json = GZipCompress.Unzip(bytes);
			json = GZipCompress.XORCipher(json, Constant.TEJAVA);

			var deserialized = JsonConvert.DeserializeObject<Dictionary<int, List<string>>>(json);

			int count = 0;
			foreach (int key in deserialized.Keys)
			{
				var hashes = deserialized[key];
				foreach (string hash in hashes)
				{
					count++;
					
					// string path = dir + $"/{hash}.{Constant.DICE_ANIM_EXTENSION}";
					// bytes = File.ReadAllBytes(path);
					// json = GZipCompress.Unzip(bytes);
					// json = GZipCompress.XORCipher(json, Constant.TEJAVA);
					//
					// RollingAnimation animObj = JsonConvert.DeserializeObject<RollingAnimation>(json);
				}
			}
			
			Debug.Log($"{count} animations loaded.");
		}
		
		#endregion
	}
}