//Create a new scriptable instance if it was deleted by mistake.

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SaltyfishKK.UriImage.Editor
{
    public static class UriImageTool
    {
        private static readonly string SettingFileDirectory = Application.dataPath + "/Resources/UriImage";


        [MenuItem("Tools/UriImage/Create New UriImage Setting")]
        public static void CreateNewSetting()
        {
            var t = typeof(UriImage_Setting);
            var GuidPaths = AssetDatabase.FindAssets("t:"+ t.FullName);
            foreach(var guid in GuidPaths)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if(path.Contains("Resources"))
                {
                    EditorUtility.DisplayDialog("Notice", "UriImage Setting is already existed, path:" + path, "ok");
                    return;
                }
            }
            if(!Directory.Exists(SettingFileDirectory))
            {
                Directory.CreateDirectory(SettingFileDirectory);
            }
            var asset = ScriptableObject.CreateInstance(t);
            AssetDatabase.CreateAsset(asset, "Assets/Resources/UriImage/UriImageSetting.asset");
            AssetDatabase.SaveAssets();
            Selection.activeObject = asset;
        }
    }

}
#endif