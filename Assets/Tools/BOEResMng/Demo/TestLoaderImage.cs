using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BOE.ResouseMng;
using BOE.ResouseMng.OnlineTexture;

public class TestLoaderImage : MonoBehaviour
{
    // LoaderManager.Instance.StartLoad(url, LoadedCallBack, Texture.width, Texture.height);
    [SerializeField] private RawImage  _rImage;
    private string _imageUrl = "http://10.251.96.32:8080/smartbank/files/icbc/products/goods/a1ac44fdd2d53d0bb21d1fd6799730f8.jfif";
	// Use this for initialization
	void Start ()
	{
		OnlineTextureManager.Instance.StartLoad(_imageUrl, (tex,url) =>
	    {
	        if (tex != null)
	        {
	            Debug.Log("url="+url);
	            _rImage.texture= tex;
	        }
	    },true );
	}
	
}
