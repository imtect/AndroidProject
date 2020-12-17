using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosalManager : MonoBehaviour
{
    public Button UnityToAndroiBtn;
    public Button AndroidToUnityBtn;

    public Transform Cube;

    AndroidGlobalService m_AndroidService;

    void Start() {
        m_AndroidService = new AndroidGlobalService();

        UnityToAndroiBtn.onClick.AddListener(() => {
            m_AndroidService.UnityCallAndroid();
        });

        AndroidToUnityBtn.onClick.AddListener(() => {
            RotateObject("5");
        });
    }

    public void RotateObject(string angle) {
        Cube.transform.Rotate(Vector3.up, float.Parse(angle));
    }
}
