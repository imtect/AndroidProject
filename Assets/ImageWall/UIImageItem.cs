using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIImageItem : MonoBehaviour, IPointerClickHandler {

    private ItemData ItemData { get; set; }

    private RawImage m_RawImage;
    private RectTransform m_Rect;

    private Vector2 m_OriginPos;

    private float m_MoveSpeed = 6f;
    private bool isMoveFinished = false;

    private bool IsCanMove { get; set; }

    public Action<ItemData> OnImageItemClicked;

    private void Awake() {
        m_Rect = GetComponent<RectTransform>();
        m_RawImage = GetComponentInChildren<RawImage>();
    }
    private void FixedUpdate() {
        if (isMoveFinished) {

            float moveX = m_Rect.anchoredPosition.x - Time.deltaTime * m_MoveSpeed;
            float moveY = m_Rect.anchoredPosition.y;
            m_Rect.anchoredPosition = new Vector2(moveX, moveY);

            if (m_Rect.anchoredPosition.x < -m_Rect.sizeDelta.x) {
                m_Rect.anchoredPosition = new Vector2(Screen.width, m_OriginPos.y);
            }
        }
    }

    public void Init(ItemData data) {
        this.ItemData = data;
    }

    public void SetImage(Texture2D texture) {
        if (m_RawImage) m_RawImage.texture = texture;
    }

    public void SetSize(float width, float height) {
        m_Rect.sizeDelta = new Vector2(width, height);
    }

    public void SetPosition(Vector2 position) {
        m_OriginPos = position;
        m_Rect.anchoredPosition = position;
    }

    public void SetPosition(Vector2 initPos, Vector2 lastPos, float time = 1) {
        m_OriginPos = initPos;
        //float length = Vector2.Distance(initPos, lastPos);
        //float time = Mathf.Lerp(0.5f, 1, length);
        DOTween.To(() => initPos, pos => { m_Rect.anchoredPosition = pos; }, lastPos, time).OnComplete(() => {
            isMoveFinished = true;
        });
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnImageItemClicked?.Invoke(ItemData);
    }
}
