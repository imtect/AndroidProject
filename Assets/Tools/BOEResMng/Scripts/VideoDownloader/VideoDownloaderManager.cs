using BOE.ResouseMng;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
namespace BOE.ResouseMng.VideoDownloader
{
    public class VideoDownloaderManager : Singleton<VideoDownloaderManager>
    {
        public void StartDownLoadVideo(string url,bool overideOld=false, Action<bool,string,string> callback = null, Action<float> downloadProgress = null)
        {
            string videoPath = BSLoadHelp.VideoDownloandDir + GetVideoNameFromURL(url);
            if (!overideOld && File.Exists(videoPath))
            {
                callback?.Invoke(true, url, videoPath);
                return;
            }
            StartCoroutine(DownLoadVideo(url, BSLoadHelp.VideoDownloandDir + GetVideoNameFromURL(url), callback, downloadProgress));
        }

        public void StartDownLoadVideo(string url, string videoPath, bool  overideOld= false, Action<bool,string,string > callback = null, Action<float> downloadProgress=null)
        {
            if (!overideOld && File.Exists(videoPath))
            {
                callback?.Invoke(true, url,videoPath);
                return;
            }
            StartCoroutine(DownLoadVideo(url, videoPath, callback, downloadProgress));
        }
        IEnumerator DownLoadVideo(string url, string videoPath, Action<bool,string,string > callback, Action<float> downloadProgress)
        {
            var uwr = new UnityWebRequest(url);
            uwr.method = UnityWebRequest.kHttpVerbGET;
            var dh = new DownloadHandlerFile(videoPath);
            dh.removeFileOnAbort = true;
            uwr.downloadHandler = dh;
            uwr.SendWebRequest();
            
            while (!uwr.isDone)
            {
                downloadProgress?.Invoke(uwr.downloadProgress);
                yield return null;
            }
            
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
                callback?.Invoke(false,url,videoPath);
            }
            else
            {
                callback?.Invoke(true,url,videoPath );
                Debug.Log("Download saved to: " + videoPath.Replace("/", "\\") + "\r\n" + uwr.error);
            }
        }
        private string GetVideoNameFromURL(string url)
        {
           if(string.IsNullOrEmpty(url))
            {
                return "";
            }
            string[] tmp = url.Split('/');
            if (tmp.Length > 0)
            {
                return tmp[tmp.Length - 1];
            }
            return "";
        }
        private void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
