using System;
using UnityEditor;
using UnityEngine;

namespace CQ.MiniGames.Editor
{
	public class SpriteProcessor : AssetPostprocessor
	{
		void OnPostprocessTexture(Texture2D texture)
		{
			
		}

		void OnPreprocessTexture()
		{
			if (assetPath.StartsWith("Assets/Sprites/"))
			{
				TextureImporter importer = (TextureImporter)assetImporter;
				
				importer.textureType = TextureImporterType.Sprite;
				importer.spriteImportMode = SpriteImportMode.Single;
				importer.filterMode = FilterMode.Bilinear;
			}
		}
	}
}