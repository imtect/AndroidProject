using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICenterDetail : MonoBehaviour {

    private Button m_CloseBtn;

    private RawImage m_RawImage;

    public Action OnCloseBtnClicked;
 
    private void Awake() {
        m_CloseBtn = transform.Find("CloseBtn").GetComponent<Button>();
        m_RawImage = transform.Find("RawImage").GetComponent<RawImage>();
        m_CloseBtn.onClick.AddListener(() => {
            gameObject.SetActive(false);
            OnCloseBtnClicked?.Invoke();
        });
    }

    public void SetDetailContent(Texture2D texture) {
        if (m_RawImage) m_RawImage.texture = texture;
    }
}
