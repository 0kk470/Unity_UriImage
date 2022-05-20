//Create a new scriptable instance if it was deleted by mistake.

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SaltyfishKK.UriImage.Editor
{
    public static class UriImageTool
    {

        [MenuItem("Tools/UriImage/Create New UriImage Setting")]
        public static void CreateNewSetting()
        {

            var asset = ScriptableObject.CreateInstance(typeof(UriImage_Setting));
            AssetDatabase.CreateAsset(asset, "Assets/Resources/UriImage/UriImageSetting.asset");
            AssetDatabase.SaveAssets();

            Selection.activeObject = asset;
        }
    }

}
#endif