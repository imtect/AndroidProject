
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace BOE.ResouseMng.LocalRes
{
    public class LocalResCacheManager : SingletonClass<LocalResCacheManager>
    {

        /// <summary>
        /// 加载文件夹中的第一个Texture2D
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="onCompleted"></param>
        public void LoadTexture(string dirPath, Action<Texture2D, string> onCompleted)
        {
            if (string.IsNullOrEmpty(dirPath)) return;
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
            FileInfo[] files = directoryInfo.GetFiles();
            if (files != null && files.Count() > 0)
            {
                var first = files[0];
                LocalTextureLoaderManager.Instance.StartLoadLocal(first.FullName, onCompleted, true);
            }
        }
        /// <summary>
        /// 加载单个文件夹中所有的Texture2D
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="onCompleted"></param>
        public IEnumerator LoadTextures(string dirPath, Action<Dictionary<string, Texture2D>> onCompleted)
        {
            if (string.IsNullOrEmpty(dirPath)) yield break;
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
            FileInfo[] files = directoryInfo.GetFiles();
            Dictionary<string, Texture2D> dic = new Dictionary<string, Texture2D>();
            files = files.Where(k => !k.FullName.Contains(".meta")).ToArray();
            foreach (var item in files)
            {
                dic.Add(item.FullName, null);
            }
            if (files != null && files.Count() > 0)
            {
                int index = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    LocalTextureLoaderManager.Instance.StartLoadLocal(files[i].FullName, (text, filePath) =>
                    {
                        if (dic.ContainsKey(filePath))
                            dic[filePath] = text;
                        ++index;
                    }, true);
                }
                while (index != files.Length)
                {
                    yield return null;
                }
                onCompleted?.Invoke(dic);
            }
        }

        public IEnumerator LoadTextures(List<string> paths, Action<float> onPercent = null, Action<Dictionary<string, Texture2D>> onCompleted = null)
        {
            Dictionary<string, Texture2D> dic = new Dictionary<string, Texture2D>();
            if (paths == null && paths.Count() == 0) yield break;
            foreach (var item in paths)
            {
                dic.Add(item, null);
            }
            int index = 0;
            for (int i = 0; i < paths.Count; i++)
            {
                LocalTextureLoaderManager.Instance.StartLoadLocal(paths[i], (text, filePath) =>
                {
                    if (dic.ContainsKey(filePath))
                        dic[filePath] = text;
                    ++index;
                    onPercent?.Invoke((float)index / paths.Count);
                }, true);
            }
            while (index != paths.Count)
            {
                yield return null;
            }
            onCompleted?.Invoke(dic);
        }

        /// <summary>
        /// 获取文件夹中第一个视频路径
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="onCompleted"></param>
        public void LoadVideoFilePaths(string dirPath, Action<List<string>> onCompleted)
        {
            if (string.IsNullOrEmpty(dirPath)) return;
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
            FileInfo[] files = directoryInfo.GetFiles("*.mp4");
            if (files != null && files.Length > 0)
            {
                var videoFiles = files.Select(k => k.FullName).ToList();
                onCompleted?.Invoke(videoFiles);
            }
        }
        public void LoadAllVideoFilePaths(string dirPath, Action<List<string>> onCompleted)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                onCompleted?.Invoke(new List<string>());
                return;
            }
            string[] paths = System.IO.Directory.GetFiles(dirPath);
            List<string> videoPath = new List<string>();
            if (paths != null)
            {
                for (int i = 0; i < paths.Length; i++)
                {
                    if (paths[i].EndsWith(".mp4"))
                    {
                        videoPath.Add(paths[i]);
                    }
                }
            }
            onCompleted?.Invoke(videoPath);
        }
    }
}
