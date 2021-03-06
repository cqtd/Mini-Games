﻿using UnityEditor;
using UnityEngine;

namespace CQ.MiniGames.Editor
{
	public class SpriteProcessor : AssetPostprocessor
	{
		private void OnPostprocessTexture(Texture2D texture)
		{
			
		}

		private void OnPreprocessTexture()
		{
			if (assetPath.StartsWith("Assets/Sprites/"))
			{
				TextureImporter importer = (TextureImporter)assetImporter;
				
				importer.textureType = TextureImporterType.Sprite;
				importer.spriteImportMode = SpriteImportMode.Single;
				importer.filterMode = FilterMode.Bilinear;
			}

			if (assetPath.ToLower().Contains("_n_") || assetPath.ToLower().Contains("_normal"))
			{
				TextureImporter importer = (TextureImporter)assetImporter;

				importer.textureType = TextureImporterType.NormalMap;
			}
		}
	}
}