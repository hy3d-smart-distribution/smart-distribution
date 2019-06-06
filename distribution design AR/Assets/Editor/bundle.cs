using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class bundle : MonoBehaviour {

    [MenuItem("Bundles/Build assetBundles")]
    static void AssetBundles() {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}
