using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageWallTest : MonoBehaviour {
    public UIImageWall UIImageWall;

    void Start() {
        UIImageWall.SetRowColumn("19, 12, 0"); //row > 5,colum是偶数且colume> 7;
        UIImageWall.SetImages("C:/Users/imtect/Desktop/Icon");
        UIImageWall.SetCenterImages("C:/Users/imtect/Desktop/Center");
    }
}
