using BOE.ResouseMng.OnlineTexture;
using BOE.ResouseMng.VideoDownloader;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BOE.ResouseMng.OnlineResCache
{
    public class OnlineResCacheManager : MonoBehaviour
    {
        private static OnlineResCacheManager instance;

        public static OnlineResCacheManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject globalObject = ObjectUtil.GetSceneObjectByName("OnlineResCacheManager");
                    instance = globalObject.AddComponent<OnlineResCacheManager>();
                }
                return instance;
            }
        }
        // Start is called before the first frame update

        private int textureTotalNum;
        private int videoTotalNum;
        int loadedTextureNum;
        int loadedVideoNum;
        Action<float>  loadProgress;
        float progressvalue;
        public  void CacheAll(List<string> textureList, List<string> videoList,Action<float> progress)
        {
            BSLoadHelp.InitResDir();
            loadedTextureNum = 0;
            loadedVideoNum = 0;
            loadProgress = progress;
            textureTotalNum = textureList != null ? textureList.Count : 0;
            videoTotalNum = videoList!=null? videoList.Count:0;
            if (textureList != null)
            {
                for (int i = 0; i < textureList.Count; i++)
                {
                    OnlineTextureManager.Instance.StartLoad(textureList[i], OnTextureLoaded,true);
                }
            }
            else
            {
                CheckToEnter();
            }

            if (videoList != null)
            {
                
                for (int i = 0; i < videoList.Count; i++)
                {
                    VideoDownloaderManager.Instance.StartDownLoadVideo(videoList[i],false , OnVideoLoaded);
                }
            }
            else
            {
                CheckToEnter();
            }
          
        }

        private void OnTextureLoaded(Texture2D texture, string url)
        {
            loadedTextureNum++;
            CheckToEnter();
        }

        private void OnVideoLoaded(bool isok,string url,string path)
        {
            loadedVideoNum++;
            CheckToEnter();
        }

        private void CheckToEnter()
        {
            progressvalue = (loadedTextureNum + loadedVideoNum)*1.00f / (textureTotalNum + videoTotalNum) * 1.00f;
            loadProgress?.Invoke(progressvalue);
            if (loadedTextureNum== textureTotalNum&& loadedVideoNum == videoTotalNum)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
