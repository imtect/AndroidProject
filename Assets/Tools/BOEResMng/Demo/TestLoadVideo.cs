using BOE.ResouseMng;
using BOE.ResouseMng.VideoDownloader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TestLoadVideo : MonoBehaviour
{
    private string url= "https://media.w3.org/2010/05/sintel/trailer.mp4";
    private string url1 = "https://www.sample-videos.com/video123/mp4/720/big_buck_bunny_720p_1mb.mp4";
    private string url2 = "https://www.sample-videos.com/video123/mp4/720/big_buck_bunny_720p_20mb.mp4";
    private string url3 = "https://www.sample-videos.com/video123/mkv/720/big_buck_bunny_720p_1mb.mkv";
    [SerializeField] private Slider vidoeProgressSlider;
    // Start is called before the first frame update
    void Start()
    {
        BSLoadHelp.InitResDir();
        VideoDownloaderManager.Instance.StartDownLoadVideo(url,false ,(isok,url,path)=>
        {
            Debug.Log("Downloaded "+isok);
        },(progress)=>
        {
            vidoeProgressSlider.value = progress;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
