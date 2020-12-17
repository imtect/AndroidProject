using UnityEngine;
using System.Collections;

public abstract class AndroidServiceBase {
    private AndroidJavaObject _java;
    public AndroidJavaObject Java {
        get {
            if (_java == null) {
                var javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _java = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return _java;
        }
    }
}
