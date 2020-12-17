using System;
using System.Collections.Generic;
using UnityEngine;

public class AndroidGlobalService : AndroidServiceBase {
    private static int _maxVolume;
    private static int _originalVolume;
    private static int _currentBrightness = 3;
    public const int MaxVolumeBenchmark = 15;

    public AndroidGlobalService() {
        //Java.CallStatic("volumeInit");
    }

    public void CloseActivity() {
        Java.Call("finish");
    }

    public void UnityCallAndroid() {
        Java.Call("PrintString", "hello,it is unity calling...");
    }

    public void AndroindCallUnity() {
        
    }


    public void GetServerTime() {
        Java.CallStatic("getServerTime");
    }

    public void SaveInitVolume() {
        _originalVolume = int.Parse(Java.CallStatic<string>("getCurrentVolume"));
    }

    public void RestoreInitVolume() {
        Java.CallStatic("setCurrentVolume", _originalVolume.ToString());
    }

    //  public int GetMaxVolume()
    //  {
    //      _maxVolume = int.Parse(Java.CallStatic<string>("getMaxVolume"));
    //      DebugHelper.Log("max volume:" + _maxVolume);
    //      if (_maxVolume > MaxVolumeBenchmark)
    //      {
    //          return MaxVolumeBenchmark;
    //      }
    //      return _maxVolume;
    //  }

    //  public void SetVolume(int volume)
    //  {
    //      var realVolume = volume;
    //      if (_maxVolume > MaxVolumeBenchmark)
    //      {
    //          realVolume = Mathf.RoundToInt(realVolume * (float)_maxVolume / MaxVolumeBenchmark);
    //      }
    //      DebugHelper.Log("set volume:" + realVolume);
    //      Java.CallStatic("setCurrentVolume", realVolume.ToString());
    //  }

    //  public int GetVolume()
    //  {
    //      var realVolume = int.Parse(Java.CallStatic<string>("getCurrentVolume"));
    //      DebugHelper.Log("get volume:" + realVolume);
    //      if (_maxVolume <= MaxVolumeBenchmark)
    //      {
    //          return realVolume;
    //      }
    //      return Mathf.RoundToInt(realVolume / ((float)_maxVolume / MaxVolumeBenchmark));
    //  }

    //  public void InitSceneBrightness()
    //  {
    //      AndroidUtility.RunOnUIThread(
    //        new AndroidJavaRunnable(() =>
    //        {
    //            Java.CallStatic("initSenceLight");
    //        }
    //      )); 


    //  }

    //  public void InitPlayerBrightness()
    //  {
    //      Java.CallStatic("initPlayLight");
    //  }

    //  public void SetBrightness(float brightness)
    //  {
    //      var b = (int) (brightness*25.5);
    //      DebugHelper.Log("Set brightness:" + b);
    //      Java.CallStatic("setLight", b.ToString());
    //  }

    //  public int GetBrightness()
    //  {
    //      var b = int.Parse(Java.CallStatic<string>("getLight"));
    //      DebugHelper.Log("Get brightness:" + b);
    //      return Mathf.CeilToInt(b/25.5f);
    //  }

    //  public NetworkStatus GetNetworkStatus()
    //  {
    //      var networkState = Java.CallStatic<string> ("getNetWorkState");
    //      return (NetworkStatus) Enum.Parse(typeof (NetworkStatus), networkState);
    //  }

    //  public bool IsHdEnabled()
    //  {
    //      return Java.CallStatic<bool>("getIsOpenHD");
    //  }

    //  public bool IsAllowGprsDownload()
    //  {
    //      return Java.CallStatic<bool>("isAllowGprsDownload");
    //  }

    //  public bool IsLogin()
    //  {
    //string userInfo = Java.CallStatic<string> ("getUserInfo");
    //string uid = JsonMapper.ToObject (userInfo)["uid"].ToString();
    //if(string.IsNullOrEmpty(uid))
    //{
    //	return false;
    //}
    //else
    //{
    //	return true;
    //}
    //  }

    //  public void Quit()
    //  {
    //Java.Call("UnityCallQuit");
    //  }

    //  public void ReportLog(ReportLogAndroidInterface.ReportType reportType, List<KeyValuePair<string, string>> parameters)
    //  {
    //      var parameterString = string.Empty;
    //      foreach (var keyValuePair in parameters)
    //      {
    //          parameterString += string.Format("\"{0}\":\"{1}\",", keyValuePair.Key, keyValuePair.Value);
    //      }
    //      parameterString = parameterString.Substring(0, parameterString.Length - 1);
    //      var json = string.Format("{{\"reportType\":\"{0}\",\"msg\":{{{1}}}}}", reportType, parameterString);
    //      Java.CallStatic("reportLog", json);
    //      DebugHelper.Log(string.Format("Call android:{0}, parameter:{1}", "reportLog", json));
    //  }

    //  /// <summary>
    //  /// get hd test result
    //  /// 0: unknow
    //  /// 1: pass
    //  /// 2: failed
    //  /// </summary>
    //  /// <returns></returns>
    //  public int GetHdTestResult()
    //  {
    //      return Java.CallStatic<int>("getHDTest");
    //  }
}
