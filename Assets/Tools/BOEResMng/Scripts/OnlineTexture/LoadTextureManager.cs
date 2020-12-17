using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using BOE.BOEComponent.Uitl;

namespace BOE.ResouseMng.OnlineTexture
{
	/// </summary>
	public class OnlineTextureManager : Singleton<OnlineTextureManager>
	{

		public static string ImageCachePath = Application.streamingAssetsPath + "/UniImageCache/";
		public const int SequenceNum = 4;

		private List<LoadSequence> sequenceList = new List<LoadSequence>();

		private Dictionary<string, Action<Texture2D, string>> loadMap;
		private Dictionary<string, Texture2D> textureDic = new Dictionary<string, Texture2D>();
		/*
		private static OnlineTextureManager instance;

		public static OnlineTextureManager Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject globalObject = ObjectUtil.GetSceneObjectByName("LoadTextureManager");
					instance = globalObject.AddComponent<OnlineTextureManager>();
				}
				return instance;
			}
		}
		*/

		//Queue<Tuple> LoadTextureRequests = new Queue<Tuple>();
		void Awake()
		{
		//	instance = this;
			LoadSequence sequece;
			for (int i = 1; i <= SequenceNum; i++)
			{
				sequece = new GameObject("Sequece" + i).AddComponent<LoadSequence>();
				sequece.transform.parent = transform;
				sequece.Init(SequeceComplete, i);
				sequenceList.Add(sequece);
			}
			loadMap = new Dictionary<string, Action<Texture2D, string>>();

			//InvokeRepeating("PopRequestStack", 0.1f,0.02f);
		}
		/// <summary>在没有Tween运行的时候队列加载图片
		/// </summary>
		//void PopRequestStack()
		//{
		//    if (LoadTextureRequests.Count > 0&&iTween.tweens.Count==0)
		//    {
		//        Hashtable ht = LoadTextureRequests.Dequeue();
		//        Debug.Log(ht["url"]);
		//        Debug.Log(ht["callback"]);
		//        LoadTexture((string)ht["url"], (Action<Texture2D>)ht["callback"], (int)ht["width"], (int)ht["hight"]);
		//    }
		//}

		void LoadTexture(string url, Action<Texture2D, string> callback, int width, int hight, bool isCacheDisk = false, float compressFactor = 1.0f)
		{
			url = url.Replace(" ", "%20");
			string extension = Path.GetExtension(url).ToUpper();
			string imgName = FileExeUtil.MD5Encrypt(url) + extension; //根据URL获取文件的名字
			if (File.Exists(ImageCachePath + imgName))
			{
				LoadLocalFile(url, callback, width, hight);
			}
			else
			{
				LoadNetFile(url, callback, isCacheDisk, compressFactor);
			}
		}
		public void SetImageCachePath(string path)
        {
			ImageCachePath = path;
		}
		/// <summary>
		/// 开始加载资源
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void StartLoad(string url, Action<Texture2D, string> callback, bool isCacheDisk = false, int loadwidth = 0, int loadhight = 0, float compressFactor = 1.0f)
		{
			//==Start========移到LoadTexture方法中============
			url = url.Replace(" ", "%20");
			string extension = Path.GetExtension(url).ToUpper();
			string imgName = FileExeUtil.MD5Encrypt(url) + extension; //根据URL获取文件的名字
			if(textureDic.ContainsKey(url)&& textureDic[url]!=null)
            {
				callback?.Invoke(textureDic[url], url);
				return;
			}
			if (File.Exists(ImageCachePath + imgName))
			{
				LoadLocalFile(url, callback, loadwidth, loadhight);
			}
			else
			{
				LoadNetFile(url, callback, isCacheDisk, compressFactor);
			}
			//==End========移到LoadTexture方法中============
		}

		public bool  HadCachedInLocal(string url)
        {
			url = url.Replace(" ", "%20");
			string extension = Path.GetExtension(url).ToUpper();
			string imgName = FileExeUtil.MD5Encrypt(url) + extension; //根据URL获取文件的名字
			if (File.Exists(ImageCachePath + imgName))
			{
				return true;
			}
			return false;
        }

		private void LoadLocalFile(string url, Action<Texture2D, string> callback, int width, int hight)
		{
			var imageCachePath = ImageCachePath;
#if UNITY_EDITOR
			string extension = Path.GetExtension(url).ToUpper();
			string imgName = FileExeUtil.MD5Encrypt(url) + extension; //根据URL获取文件的名字
			FileStream fs = File.OpenRead(imageCachePath + imgName); //OpenRead
			int filelength = (int)fs.Length; //获得文件长度 
			var image = new Byte[filelength]; //建立一个字节数组 
			fs.Read(image, 0, filelength); //按字节流读取 

			var text = new Texture2D(width, hight);
			text.LoadImage(image);
			callback(text, url);
			if(!textureDic.ContainsKey(url))
            {
				textureDic.Add(url, text);
			}
#else
        Loom.RunAsync(() =>
	    {
            string extension = Path.GetExtension(url).ToUpper();
            string imgName = FileExeUtil.MD5Encrypt(url) + extension; //根据URL获取文件的名字
            FileStream fs = File.OpenRead(imageCachePath + imgName); //OpenRead
            int filelength = (int)fs.Length; //获得文件长度 
            var image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            Loom.QueueOnMainThread(() =>
            {
                var text = new Texture2D(width, hight);
                text.LoadImage(image);
                callback(text,url);
			   if(!textureDic.ContainsKey(url))
				{
					textureDic.Add(url, text);
				}
            });
	    });
#endif

		}

		private void LoadNetFile(string url, Action<Texture2D, string> callback, bool isCacheDisk, float compressFactor)
		{
			if (loadMap.ContainsKey(url))
			{
				loadMap[url] = callback;
				return;
			}
			//选择序列
			LoadSequence sequence = GetSequence();
			if (sequence != null)
			{
				//			Debug.LogWarning("chose index:" + sequence.index);
				sequence.AddLoad(url, isCacheDisk, compressFactor);
				loadMap.Add(url, callback);
			}
			else
			{
				Debug.LogWarning("下载序列获取失败。");
			}
		}


		/// <summary>
		/// 加载前，清除掉之前所有的下载序列
		/// </summary>
		public void LoadByExclusive()
		{
			Debug.LogError("未实现");
		}

		private LoadSequence GetSequence()
		{
			LoadSequence sequence = null;
			foreach (LoadSequence tempSequence in sequenceList)
			{
				if (sequence == null || tempSequence.LoadNum < sequence.LoadNum)
				{
					sequence = tempSequence;
				}
			}
			return sequence;
		}

		//	private bool IsFilter(string url)
		//	{
		//		if ( loadMap.ContainsKey (url) )
		//		{
		//		}
		//	}

		private void SequeceComplete(string url, Texture2D texture)
		{
			if (loadMap.ContainsKey(url))
			{
				if (loadMap[url] != null)
				{
					loadMap[url](texture, url);
					if (!textureDic.ContainsKey(url)&&texture!=null)
					{
						textureDic.Add(url, texture);
					}
				}

				loadMap.Remove(url);
			}
		}
	}
}
