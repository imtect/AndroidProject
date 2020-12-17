///<summary>
///<para>Copyright (C)</para>
/// <para>文件功能：</para>
/// <para>创 建 人：范海军 </para>
/// <para>电子邮件： </para>
/// <para>创建日期：2019-8-13</para>
/// <para>修 改 人：</para>
/// <para>修改日期：</para>
/// <para>备    注：</para>
/// </summary>

using BOE.BOEComponent.Uitl;
using BOE.BOEComponent.Util;
using BOE.ResouseMng;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
namespace BOE.ResouseMng.LocalRes
{

        public class LocalTextureLoaderManager : Singleton<LocalTextureLoaderManager>
        {
            public Dictionary<string, Texture2D> TextureCache = new Dictionary<string, Texture2D>();

            public void StartLoadResInResources(string path, Action<GameObject> callback)
            {
                StartCoroutine(LoadResInResources(path, callback));
            }

            private IEnumerator LoadResInResources(string path, Action<GameObject> callback)
            {
                var request = Resources.LoadAsync(path);
                yield return request;
                if (request.isDone)
                {
                    GameObject res = request.asset as GameObject;
                    if (callback != null)
                    {
                        callback(res);
                    }
                }
            }

            public void StartLoadTextureInResources(string path, Action<Texture2D, string> callback)
            {
                StartCoroutine(LoadTextureInResources(path, callback));
            }

            private IEnumerator LoadTextureInResources(string imagePath, Action<Texture2D, string> callback)
            {
                var request = Resources.LoadAsync(imagePath);
                yield return request;
                if (request.isDone)
                {
                    Texture2D iconImage = (Texture2D)request.asset;
                    if (callback != null)
                    {
                        callback(iconImage, imagePath);
                    }
                }
            }

            public Texture2D GetTextureFromCache(string path)
            {
                if (TextureCache.ContainsKey(path))
                {
                    return TextureCache[path];
                }
                else
                {
                    return null;
                }
            }


            /// <summary>
            /// 本地加载纹理贴图
            /// </summary>
            /// <param name="texturePaths"></param>
            /// <param name="usecache"></param>
            /// <param name="onPercent"></param>
            /// <param name="onComplete"></param>
            public void LoadTextures(List<string> texturePaths, bool usecache = false, Action<float> onPercent = null, Action onComplete = null)
            {
                Loom.RunAsync(() =>
                {
                    for (int i = 0; i < texturePaths.Count; i++)
                    {
                        var filePath = texturePaths[i];
                        if (File.Exists(filePath))
                        {
                            var image = File.ReadAllBytes(texturePaths[i]);
                            Thread.Sleep(100);
                            var percent = ((float)i / (texturePaths.Count - 1)) * 100;
                            Loom.QueueOnMainThread(() =>
                            {
                                var texture2D = new Texture2D(0, 0);
                                texture2D.LoadImage(image);
                                if (usecache && i < texturePaths.Count && !TextureCache.ContainsKey(filePath))
                                {
                                    TextureCache.Add(filePath, texture2D);
                                }
                                onPercent?.Invoke(percent);

                                if (i == texturePaths.Count)
                                {
                                    onComplete?.Invoke();
                                }
                            });
                        }
                        else
                        {
                            Debug.LogError("File NotExists : " + filePath);
                        }
                    }
                });
            }


            public void StartLoadLocal(string path, Action<Texture2D, string> callback, bool cheCheInMemery = false)
            {
                if (File.Exists(path))
                {
                    if (TextureCache.ContainsKey(path))
                    {
                        callback?.Invoke(TextureCache[path], path);
                        return;
                    }
                    Loom.RunAsync(() =>
                    {
                        var image = File.ReadAllBytes(path);
                        Loom.QueueOnMainThread(() =>
                        {
                            var text = new Texture2D(0, 0);
                            text.LoadImage(image);

                            if (cheCheInMemery && !TextureCache.ContainsKey(path))
                            {
                                TextureCache.Add(path, text);
                            }
                            callback.Invoke(text, path);
                        });
                    });
                }
                else
                {
                    Debug.Log("File NotExists : " + path);
                    callback?.Invoke(null, null);
                }
            }

            public bool IsLoaded(List<string> list)
            {
                if (list == null || list.Count == 0) return false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (!TextureCache.ContainsKey(list[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
}
