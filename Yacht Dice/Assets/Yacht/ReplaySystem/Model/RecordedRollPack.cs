using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Yacht.Gameplay.ReplaySystem
{
	[CreateAssetMenu(menuName = "Replay/Roll Pack", fileName = "RecordedRollPack", order = 50)]
	public class RecordedRollPack : ScriptableObject
	{
		public List<RecordedRoll> dice1;
		public List<RecordedRoll> dice2;
		public List<RecordedRoll> dice3;
		public List<RecordedRoll> dice4;
		public List<RecordedRoll> dice5;

		private string extension = "bda";

		[ContextMenu("Export binary", false, 100)]
		private void Export()
		{
			var animationSet1 = dice1.Select(e => e.Convert()).ToArray();
			var animationSet2 = dice2.Select(e => e.Convert()).ToArray();
			var animationSet3 = dice3.Select(e => e.Convert()).ToArray();
			var animationSet4 = dice4.Select(e => e.Convert()).ToArray();
			var animationSet5 = dice5.Select(e => e.Convert()).ToArray();
			
			string dir = Application.streamingAssetsPath + "/Patchable";
			var dirInfo = new DirectoryInfo(dir);
			
			if (!dirInfo.Exists) dirInfo.Create();

			Dictionary<int, List<string>> map = new Dictionary<int, List<string>>();
			
			List<string> hashes = new List<string>();
			foreach (RollingAnimation rollData in animationSet1)
			{
				string hash = GZipCompress.Hasing("dice.animation", $"{rollData.length}_1");
				string serialzied = JsonConvert.SerializeObject(rollData, Formatting.None);
				serialzied = GZipCompress.XORCipher(serialzied, "tejava");

				File.WriteAllBytes(dir + $"/{hash}.{extension}", GZipCompress.Zip(serialzied));

				hashes.Add(hash);
			}

			map[1] = hashes;
			hashes = new List<string>();
			
			foreach (RollingAnimation rollData in animationSet2)
			{
				string hash = GZipCompress.Hasing("dice.animation", $"{rollData.length}_2");
				string serialzied = JsonConvert.SerializeObject(rollData, Formatting.None);
				serialzied = GZipCompress.XORCipher(serialzied, "tejava");

				File.WriteAllBytes(dir + $"/{hash}.{extension}", GZipCompress.Zip(serialzied));
					
				hashes.Add(hash);
			}

			map[2] = hashes;
			hashes = new List<string>();
			
			foreach (RollingAnimation rollData in animationSet3)
			{
				string hash = GZipCompress.Hasing("dice.animation", $"{rollData.length}_3");
				string serialzied = JsonConvert.SerializeObject(rollData, Formatting.None);
				serialzied = GZipCompress.XORCipher(serialzied, "tejava");
				
				File.WriteAllBytes(dir + $"/{hash}.{extension}", GZipCompress.Zip(serialzied));
					
				hashes.Add(hash);
			}

			map[3] = hashes;
			hashes = new List<string>();
			
			foreach (RollingAnimation rollData in animationSet4)
			{
				string hash = GZipCompress.Hasing("dice.animation", $"{rollData.length}_4");
				string serialzied = JsonConvert.SerializeObject(rollData, Formatting.None);
				serialzied = GZipCompress.XORCipher(serialzied, "tejava");

				File.WriteAllBytes(dir + $"/{hash}.{extension}", GZipCompress.Zip(serialzied));
				
				hashes.Add(hash);
			}

			map[4] = hashes;
			hashes = new List<string>();
			
			foreach (RollingAnimation rollData in animationSet5)
			{
				string hash = GZipCompress.Hasing("dice.animation", $"{rollData.length}_5");
				string serialzied = JsonConvert.SerializeObject(rollData, Formatting.None);
				serialzied = GZipCompress.XORCipher(serialzied, "tejava");

				File.WriteAllBytes(dir + $"/{hash}.{extension}", GZipCompress.Zip(serialzied));
					
				hashes.Add(hash);
			}

			map[5] = hashes;
			
			string serializedMap = JsonConvert.SerializeObject(map, Formatting.None);
			serializedMap = GZipCompress.XORCipher(serializedMap, "tejava");
			
			File.WriteAllBytes(dir + $"/hash.bin", GZipCompress.Zip(serializedMap));
		}

		[ContextMenu("Unzip Hash", false, 101)]
		private void UnzipHash()
		{
			string dir = Application.streamingAssetsPath + "/Patchable";
			
			byte[] bytes = File.ReadAllBytes(dir + $"/hash.bin");
			string json = GZipCompress.Unzip(bytes);
			json = GZipCompress.XORCipher(json, "tejava");

			var deserialized = JsonConvert.DeserializeObject<Dictionary<int, List<string>>>(json);

			int count = 0;
			foreach (int key in deserialized.Keys)
			{
				var hashes = deserialized[key];
				foreach (string hash in hashes)
				{
					count++;
					
					string path = dir + $"/{hash}.{extension}";
					bytes = File.ReadAllBytes(path);
					json = GZipCompress.Unzip(bytes);
					json = GZipCompress.XORCipher(json, "tejava");

					RollingAnimation animObj = JsonConvert.DeserializeObject<RollingAnimation>(json);
				}
			}
			
			Debug.Log($"{count} animations loaded.");
		}
	}
}