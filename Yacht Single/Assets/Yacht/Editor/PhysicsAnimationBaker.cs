﻿using UnityEditor;
using UnityEngine;
using Yacht.ReplaySystem;

namespace CQ.MiniGames.Editor
{
	using Yacht.Gameplay;
	
	public class PhysicsAnimationBaker : EditorWindow
	{
		[MenuItem("Tools/Physics/Animation Baker")]
		private static void CreateWindow()
		{
			CreateWindow<PhysicsAnimationBaker>("Physics Animation Baker");
		}

		private void Create()
		{
			string path = EditorUtility.SaveFilePanelInProject(
				"Save at",
				"Physics Animation Pack",
				"asset",
				"저장할 위치를 선택하세요.",
				"Assets/Animations");

			RecordedRollPack container = CreateInstance<RecordedRollPack>();
			
			
		}
	}
}