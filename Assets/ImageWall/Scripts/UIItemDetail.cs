using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDetail : MonoBehaviour {

    private RawImage m_RawImage;
    private Button m_CloseBtn;

    public Action OnCloseBtnClicked;

    private void Awake() {
        m_RawImage = transform.Find("RawImage").GetComponent<RawImage>();
        m_CloseBtn = transform.Find("CloseBtn").GetComponent<Button>();

        m_CloseBtn.onClick.AddListener(() => {
            gameObject.SetActive(false);
            OnCloseBtnClicked?.Invoke(); 
        });
    }

    public void SetDetailContent(Texture2D texture) {
        if (m_RawImage) m_RawImage.texture = texture;
    }
}
