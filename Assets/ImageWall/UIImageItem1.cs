using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIImageItem1 : MonoBehaviour, IPointerClickHandler {

    public ItemData ItemData { get; set; }
    public Vector2 OriginPos { get; set; }

    private Text m_Text;

    private RawImage m_RawImage;
    private RectTransform m_Rect;
    public Action<ItemData> OnImageItemClicked;

    private bool m_IsFinished;
    private float moveSpeed = 100f;

    private void Awake() {
        m_Rect = GetComponent<RectTransform>();
        m_RawImage = GetComponentInChildren<RawImage>();

        m_Text = GetComponentInChildren<Text>();
    }

    private void Update() {
        //if (!m_IsFinished) return;

        //m_Rect.Translate(new Vector2(-1, 0) * Time.deltaTime * moveSpeed);
        ////当X轴移动到最左边时，对象更新位置到最右边
        //if (m_Rect.anchoredPosition.x < -m_Rect.sizeDelta.x) {
        //    m_Rect.anchoredPosition = new Vector2(Screen.width, OriginPos.y);
        //}

    }

    public void Init(ItemData data) {
        this.ItemData = data;
    }

    public void SetImage(Texture2D texture) {
        if (m_RawImage) m_RawImage.texture = texture;
    }

    public void SetText(string text) {
        if (m_Text) m_Text.text = text;
    }

    public void SetSize(float width, float height) {
        m_Rect.sizeDelta = new Vector2(width, height);
    }

    public void SetPosition(Vector2 initPos, Vector2 lastPos, float time = 1) {
        OriginPos = initPos;
        DOTween.To(() => initPos, pos => { m_Rect.anchoredPosition = pos; }, lastPos, time).OnComplete(()=> {
            m_IsFinished = true;
        });
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnImageItemClicked?.Invoke(ItemData);
    }
}
