using BOE.ResouseMng.LocalRes;
using BOE.ResouseMng.OnlineTexture;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOE.ResouseMng
{
    public class BOETextureLoaderCenter : Singleton<BOETextureLoaderCenter>
    {

        //    public void StartLoadLocal(string path, Action<Texture2D, string> callback, bool cheCheInMemery = false)
        //	public void StartLoad(string url, Action<Texture2D, string> callback, bool isCacheDisk = false, int loadwidth = 0, int loadhight = 0, float compressFactor = 1.0f)
        public void StartLoadLocalAndOnlineTexure(string  path, Action<Texture2D, string> callback,bool cheCheInMemery=true, bool isCacheDisk = false)
        {
            if(string.IsNullOrEmpty(path))
            {
                callback?.Invoke(null, path);
                return;
            }
            if(path.StartsWith("http")|| path.StartsWith("https"))
            {
                OnlineTextureManager.Instance.StartLoad(path, callback, isCacheDisk);
            }
            else
            {
                LocalTextureLoaderManager.Instance.StartLoadLocal(path, callback, cheCheInMemery);
            }
        }
    }
}
