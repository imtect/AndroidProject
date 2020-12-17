using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using BOE.BOEComponent.Uitl;

namespace BOE.ResouseMng.OnlineTexture
{
    public class LoadSequence : MonoBehaviour
    {

        public enum LoadState
        {
            run,
            stop
        }

        public int LoadNum { get { return loadList.Count; } }
        public int index;

        private LoadState state = LoadState.stop;

        private Action<string, Texture2D> CompleteCallback;
        private Action<string, Texture2D> ProgressCallback;

        private List<string> loadList = new List<string>();
        private Dictionary<string, float> _compressFactorDictionary = new Dictionary<string, float>();
        private Dictionary<string, bool> _isCacheDiskDictionary = new Dictionary<string, bool>();
        private UnityWebRequest loadRequst;

        public void Init(Action<string, Texture2D> CompleteCallback, int index)
        {
            this.CompleteCallback = CompleteCallback;
            this.index = index;
        }

        public void AddLoad(string url, bool isCacheDisk, float compressFactor)
        {
            loadList.Add(url);
            if (!_compressFactorDictionary.ContainsKey(url))
            {
                _compressFactorDictionary.Add(url, compressFactor);
            }
            if (!_isCacheDiskDictionary.ContainsKey(url))
            {
                _isCacheDiskDictionary.Add(url, isCacheDisk);
            }
            if (state == LoadState.stop)
            {
                StartLoad();
            }
        }

        public void StartLoad()
        {
            state = LoadState.run;
            string url = loadList[0];
            string extension = Path.GetExtension(url).ToUpper();
            string imgName = FileExeUtil.MD5Encrypt(url) + extension;
            string path = OnlineTextureManager.ImageCachePath + imgName;
            StartCoroutine(DownLoad(url, path));
        }

        protected IEnumerator DownLoad(string url, string path)
        {

            loadRequst = UnityWebRequest.Get(url);
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            loadRequst.downloadHandler = downloadTexture;
            yield return loadRequst.SendWebRequest();
            //loadWWW = new WWW(url);
            // yield return loadWWW;
            if (loadRequst.error == null)
            {
                try
                {
                    //  Texture2D texture = FileExeUtil.ScaleTexture(loadWWW.texture, (int)(loadWWW.texture.width / 2.5), (int)(loadWWW.texture.height / 2.5));
                    Texture2D texture = downloadTexture.texture;
                    if (_compressFactorDictionary.ContainsKey(url) && 1.0f - _compressFactorDictionary[url] > 0.1f)
                    {
                        texture = FileExeUtil.ScaleTexture(downloadTexture.texture, (int)(downloadTexture.texture.width * _compressFactorDictionary[url]), (int)(downloadTexture.texture.height * _compressFactorDictionary[url]));
                    }

                    if (CompleteCallback != null)
                    {
                        CompleteCallback(url, texture);
                    }
                    if (!Directory.Exists(FileCacheManager.ImageCachePath))
                    {
                        Directory.CreateDirectory(FileCacheManager.ImageCachePath);
                    }
                    Debug.Log("ImageCachePath" + FileCacheManager.ImageCachePath);
                    if (_isCacheDiskDictionary[url])
                    {
                        byte[] bytes = FileExeUtil.EncodeTexture(texture, path);
                        if (bytes == null)
                        {
                            bytes = loadRequst.downloadHandler.data;
                        }
                        SaveAsync(bytes, path);
                    }

                }
                catch (Exception e)
                {
                    Debug.Log("!!!!!!!!!!!DownLoadToLocal:" + e.ToString());
                    if (CompleteCallback != null)
                    {
                        CompleteCallback(url, downloadTexture.texture);
                    }
                }
            }
            else
            {
                Debug.Log("DownLoad Image url : " + url + "  error : " + loadRequst.error);
                if (CompleteCallback != null)
                {
                    CompleteCallback(url, null);
                }
            }
            //		loadWWW.Dispose ();
            Resources.UnloadUnusedAssets();
            loadRequst = null;
            loadList.Remove(url);
            if (loadList.Count <= 0)
            {
                state = LoadState.stop;
            }
        }

        private void SaveAsync(byte[] data, string path)
        {
            Loom.RunAsync(() =>
            {
                using (FileStream fs = File.Create(path))
                {
                    fs.Write(data, 0, data.Length);
                    fs.Close();
                }
            });
        }

        void Update()
        {
            if (loadRequst != null)
            {
                if (state == LoadState.run)
                {
                    //@超时判断
                    //				Debug.Log("[s." + index + "] Update progress : " + loadWWW.progress);
                }
                else
                {

                }
            }
            else if (state == LoadState.run)
            {
                StartLoad();
            }
        }
    }
}
