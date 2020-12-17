using BOE.ResouseMng.OnlineResCache;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCaheAllOnlineRes : MonoBehaviour
{
    [SerializeField] private Slider vidoeProgressSlider;
    private string[] images = new string[] { "http://10.251.96.32:8080/smartbank/files/icbc/products/goods/a1ac44fdd2d53d0bb21d1fd6799730f8.jfif" };
    private string[] videoUrls = new string[]{
                    "https://media.w3.org/2010/05/sintel/trailer.mp4",
    };
  
    // Start is called before the first frame update
    void Start()
    {

        OnlineResCacheManager.Instance.CacheAll(new List<string>(images),new List<string>(videoUrls),(progress)=>
        {
            vidoeProgressSlider.value = progress;
        });
    }

}
