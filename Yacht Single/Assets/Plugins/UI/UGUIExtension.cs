using CQ.MiniGames;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public static class UGUIExtension
{
	public static void SetAlpha(this Graphic image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		
		image.color = color;
	}
}