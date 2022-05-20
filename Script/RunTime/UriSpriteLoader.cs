using SaltyfishKK.Util;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SaltyfishKK.UriImage
{
    public class UriSpriteLoader:Singleton<UriSpriteLoader>
    {
        private Sprite m_DefaultErrorSprite;

        private Dictionary<string, Sprite> m_CacheSprites = new Dictionary<string, Sprite>();

        private Dictionary<Image, UnityWebRequest> m_LoadRequests = new Dictionary<Image, UnityWebRequest>();

        private List<Image> m_ImagesAlreadyDestroyed = new List<Image>();

        public void ClearAllCacheSprites()
        {
            m_DefaultErrorSprite = null;
            m_CacheSprites.Clear();
        }

        private bool IsRequesting(Image img)
        {
            bool contains = m_LoadRequests.ContainsKey(img);
            bool isActive = img != null && img.gameObject.activeInHierarchy;
            if (contains && !isActive)
            {
                EndRequest(img);
            }
            return contains && isActive;
        }


        private void CheckDestroyedImages()
        {
            foreach(var img in m_LoadRequests.Keys)
            {
                if (img == null)
                    m_ImagesAlreadyDestroyed.Add(img);
            }
            if(m_ImagesAlreadyDestroyed.Count > 0)
            {
                foreach(var img in m_ImagesAlreadyDestroyed)
                {
                    EndRequest(img);
                    Debug.Log("Clear:" + img);
                }
                m_ImagesAlreadyDestroyed.Clear();
            }
        }


        public void DisplayFromFilePath(Image img, string filePath, bool isNative = false)
        {
            if (string.IsNullOrEmpty(filePath))
                return;
            if(!File.Exists(filePath))
            {
                DisplayErrorImage(img);
                return;
            }
            DisplayFromRemote(img, "file://" + filePath, isNative); 
        }

        public void DisplayFromRemote(Image img, string uri, bool isNative = false)
        {
            if (string.IsNullOrEmpty(uri))
            {
                DisplayErrorImage(img);
                return;
            }
            if (!m_CacheSprites.TryGetValue(uri, out Sprite sprite))
            {
                if (IsRequesting(img))
                    return;
                if(!img.gameObject.activeInHierarchy)
                {
                    DisplayErrorImage(img);
                    Debug.LogFormat("[{0}] is not active", img);
                    return;
                }
                img.StartCoroutine(Co_LoadSpriteFromUri(uri, img, isNative));
            }
            else
            {
                img.sprite = sprite;
            }
            CheckDestroyedImages();
        }

        private void DisplayErrorImage(Image img)
        {
            if (!UriImage_Setting.EnableErrorImage)
                return;
            if (m_DefaultErrorSprite == null)
                m_DefaultErrorSprite = Resources.Load<Sprite>(UriImage_Setting.DefaultErrorImagePath);
            img.sprite = m_DefaultErrorSprite;
        }


        private UnityWebRequestAsyncOperation BeginRequest(Image img, UnityWebRequest request)
        {
            if (!m_LoadRequests.ContainsKey(img))
                m_LoadRequests.Add(img, request);
            return request.SendWebRequest();
        }

        public void EndRequest(Image img)
        {
            if (m_LoadRequests.ContainsKey(img))
            {
                m_LoadRequests[img]?.Abort();
                m_LoadRequests.Remove(img);
            }
        }

        private Sprite CacheTexture(string uri, Texture2D texture)
        {
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            if(m_CacheSprites.ContainsKey(uri))
            {
                m_CacheSprites[uri] = sprite;
            }
            else
            {
                m_CacheSprites.Add(uri, sprite);
            }
            return sprite;
        }


        private IEnumerator Co_LoadSpriteFromUri(string uri, Image img, bool isNative)
        {
            var request = UnityWebRequestTexture.GetTexture(uri);
            yield return BeginRequest(img, request);
#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.Log(request.error);
                DisplayErrorImage(img);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(request);
                img.sprite = CacheTexture(uri, texture);
                if (isNative)
                    img.SetNativeSize();
            }
            EndRequest(img);
        }
    }
}
