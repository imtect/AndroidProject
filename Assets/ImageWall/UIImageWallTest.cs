using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageWallTest : MonoBehaviour {
    public UIImageWall1 UIImageWall;

    void Start() {
        UIImageWall.SetRowColumn("24, 16, 5"); //row > 5,colum是偶数且colume> 7;
        UIImageWall.SetImages("C:/Users/10410543/Desktop/Icon");
        UIImageWall.SetCenterImages("C:/Users/10410543/Desktop/Center");
    }
}
