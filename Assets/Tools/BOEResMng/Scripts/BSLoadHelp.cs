using BOE.ResouseMng.OnlineTexture;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace BOE.ResouseMng
{
    public class BSLoadHelp
    {
        private const string RootDir = "BOERes";
        private const string OnlineTextureCache = "OnlineTextureCache";
        private const string VideoDir = "Video";
        private const string LocalTexture = "LocalTexture";
        public static string OnlineTextureCacheDir;
        public static string VideoDownloandDir;
        public static string LocalTextureDir;
        public static void InitResDir()
        {
            string parent = Directory.GetParent(Application.dataPath).FullName;
            string rootDirFull = Path.Combine(parent, RootDir);
            OnlineTextureCacheDir = Path.Combine(rootDirFull, OnlineTextureCache) +"/";
            VideoDownloandDir = Path.Combine(rootDirFull, VideoDir)+"/";
            LocalTextureDir= Path.Combine(rootDirFull, LocalTexture) + "/";
            CreateDir(rootDirFull);
            CreateDir(OnlineTextureCacheDir);
            CreateDir(VideoDownloandDir);
            CreateDir(LocalTextureDir);
            OnlineTextureManager.Instance.SetImageCachePath(OnlineTextureCacheDir);
        }

        public static void DelectAllCacheRes()
        {
            string parent = Directory.GetParent(Application.dataPath).FullName;
            string rootDirFull = Path.Combine(parent, RootDir);
            if (Directory.Exists(rootDirFull))
            {
                try
                {
                    Directory.Delete(rootDirFull,true);
                }
                catch
                {

                }
            }
        }
        private static void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
