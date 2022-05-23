using SaltyfishKK.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SaltyfishKK.UriImage
{
    [Serializable]
    public struct UriImageParam
    {
        public bool IsNativeSize;

        public bool IsOverrideSprite;

    }

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


        public void DisplayFromFilePath(Image img, string filePath, UriImageParam param = default)
        {
            if (string.IsNullOrEmpty(filePath))
                return;
            if(!File.Exists(filePath))
            {
                DisplayErrorImage(img, param);
                return;
            }
            DisplayFromRemote(img, "file://" + filePath, param); 
        }

        public void DisplayFromRemote(Image img, string uri, UriImageParam param = default)
        {
            if (string.IsNullOrEmpty(uri))
            {
                DisplayErrorImage(img, param);
                return;
            }
            if (!m_CacheSprites.TryGetValue(uri, out Sprite sprite))
            {
                if (IsRequesting(img))
                    return;
                if(!img.gameObject.activeInHierarchy)
                {
                    DisplayErrorImage(img, param);
                    Debug.LogFormat("[{0}] is not active", img);
                    return;
                }
                img.StartCoroutine(Co_LoadSpriteFromUri(uri, img, param));
            }
            else
            {
                RefreshSprite(img, sprite, param);
            }
            CheckDestroyedImages();
        }

        private void DisplayErrorImage(Image img, UriImageParam param)
        {
            if (!UriImage_Setting.EnableErrorImage)
                return;
            if (m_DefaultErrorSprite == null)
                m_DefaultErrorSprite = Resources.Load<Sprite>(UriImage_Setting.DefaultErrorImagePath);
            RefreshSprite(img, m_DefaultErrorSprite, param, true);
        }


        private UnityWebRequestAsyncOperation BeginRequest(Image img, UnityWebRequest request)
        {
            if (!m_LoadRequests.ContainsKey(img))
                m_LoadRequests.Add(img, request);
            else
                m_LoadRequests[img] = request;
            return request.SendWebRequest();
        }


        public void EndRequest(Image img)
        {
            if (m_LoadRequests.ContainsKey(img))
            {
                m_LoadRequests[img]?.Abort();
                m_LoadRequests[img]?.Dispose();
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


        private IEnumerator Co_LoadSpriteFromUri(string uri, Image img, UriImageParam param)
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
                DisplayErrorImage(img, param);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(request);
                var sprite = CacheTexture(uri, texture);

                RefreshSprite(img, sprite, param);
            }
            EndRequest(img);
        }

        private void RefreshSprite(Image img, Sprite sprite, UriImageParam param, bool isError = false)
        {
            if (param.IsOverrideSprite)
                img.overrideSprite = sprite;
            else
                img.sprite = sprite;
            if (param.IsNativeSize && !isError)
            {
                img.SetNativeSize();
            }
        }
    }
}
