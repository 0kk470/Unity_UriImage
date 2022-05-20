using UnityEngine.UI;
using UnityEngine;

namespace SaltyfishKK.UriImage
{
    public enum UriSourceType
    {
        Remote,
        LocalFile,
    }

    [AddComponentMenu("UI/UriImage", 53)]
    [DisallowMultipleComponent]
    public class UriImage : Image
    {
        [SerializeField]
        private UriSourceType m_UriType = UriSourceType.Remote;

        [SerializeField]
        private string m_Uri;

        [SerializeField]
        private bool m_LoadOnStart = false;

        [SerializeField]
        private bool m_SetNativeSize = true;

        protected override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            if (m_LoadOnStart)
                BeginLoadSprite();
        }

        public void LoadSpriteFromUri(string uri, UriSourceType sourceType = UriSourceType.Remote)
        {
            m_Uri = uri;
            m_UriType = sourceType;
            BeginLoadSprite();
        }


        [ContextMenu("BeginLoadSprite")]
        private void BeginLoadSprite()
        {
            if (m_UriType == UriSourceType.Remote)
            {
                UriSpriteLoader.Instance.DisplayFromRemote(this, m_Uri, m_SetNativeSize);
            }
            else
            {
                UriSpriteLoader.Instance.DisplayFromFilePath(this, m_Uri, m_SetNativeSize);
            }
        }
    }
}
