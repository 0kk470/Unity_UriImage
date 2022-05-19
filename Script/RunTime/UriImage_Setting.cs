using System.Collections;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

namespace SaltyfishKK.UriImage
{
    public class UriImage_Setting : ScriptableObject
    {

        private static UriImage_Setting m_Instance;

        public static UriImage_Setting Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = Resources.Load<UriImage_Setting>("UriImage/UriImageSetting");
                }
                return m_Instance;
            }
        }

        public static bool EnableErrorImage
        {
            get
            {
                return Instance?.m_EnableErrorImage ?? false;
            }
        }

        [SerializeField]
        private bool m_EnableErrorImage;

        [SerializeField]
        private string m_DefaultErrorImagePath = "UriImage/no_image";

        public static string DefaultErrorImagePath => Instance?.m_DefaultErrorImagePath ?? string.Empty;

    }
}