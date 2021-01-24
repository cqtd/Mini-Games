using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace Yacht.Editor
{
    public class AssetBundleBuilder
    {
        [MenuItem("Bundles/Build")]
        private static void BuildAssetBundles()
        {
            BuildPipeline.BuildAssetBundles("Assets/_AssetBundle/",
                BuildAssetBundleOptions.None,
                BuildTarget.Android);
        }
    }
}
