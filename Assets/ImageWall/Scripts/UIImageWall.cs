using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BOE.ResouseMng.LocalRes;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class UIImageWall : MonoBehaviour {

    #region 变量

    private float m_Width;
    private float m_Height;

    private int m_Row;
    private int m_DisplayColumn; //显示的列数
    private int m_Column; //m_DisplayColumn + 1
    private float m_ImageSpace;

    private float m_ImageItemWidth;
    private float m_ImageItemHeight;

    private UIImageItem m_UIImageItem;
    private Transform m_CenterImage;

    private List<ItemData> m_ItemDatas = new List<ItemData>();
    private Dictionary<ItemData, UIImageItem> m_UIImageItems = new Dictionary<ItemData, UIImageItem>();

    private UICenterDetail m_UICenterDetial;
    private UIItemDetail m_UIItemDetail;

    private void Awake() {
        this.m_Width = Screen.width;
        this.m_Height = Screen.height;

        m_UIImageItem = transform.Find("ImageItem").GetComponent<UIImageItem>();
        if (m_UIImageItem) m_UIImageItem.gameObject.SetActive(false);

        m_CenterImage = transform.Find("CenterImage");
        if (m_CenterImage) m_CenterImage.gameObject.SetActive(false);
    }

    private void FixedUpdate() {
        RefreshItemPosition();
    }

    #endregion

    #region 公开

    public void SetRowColumn(string data) {
        if (string.IsNullOrEmpty(data)) return;
        string[] sprites = data.Split(',');

        int row = int.Parse(sprites[0]);
        int columne = int.Parse(sprites[1]);
        int imageSpace = int.Parse(sprites[2]);

        this.m_Row = row;
        this.m_DisplayColumn = columne;
        this.m_Column = columne + 1;
        this.m_ImageSpace = imageSpace;
        this.m_ImageItemWidth = (m_Width - (m_DisplayColumn - 1) * imageSpace) / m_DisplayColumn;
        this.m_ImageItemHeight = (m_Height - (m_Row - 1) * imageSpace) / m_Row;

        this.InitConfig();
        this.InitPosition();
    }

    public void SetImages(string dirPath) {
        if (string.IsNullOrEmpty(dirPath)) return;
        StartCoroutine(LocalResCacheManager.Instance.LoadTextures(dirPath, (texs) => {
            SetImages(texs.Values.ToList());
        }));
    }

    public void SetCenterImages(string centerImagePath) {
        if (string.IsNullOrEmpty(centerImagePath)) return;
        LocalResCacheManager.Instance.LoadTexture(centerImagePath, (texs, str) => {
            GameObject centerImage = Instantiate(m_CenterImage, transform).gameObject;
            centerImage.SetActive(true);
            centerImage.GetComponentInChildren<RawImage>().texture = texs;
            centerImage.GetComponent<RectTransform>().sizeDelta = new Vector2(2 * m_ImageItemWidth, 2 * m_ImageItemHeight);
            centerImage.GetComponent<Button>().onClick.AddListener(() => {
                //显示中心详情页 //temp
                SetShowDetailEffect();
                CreateCenterDetail(texs);
            });
        });
    }

    #endregion

    #region 私有
    private int centerRow;
    private int centerColumn;

    private int startRow;
    private int endRow;

    private int startColumn;
    private int endColumn;

    private float maxHeightOffset;
    private float minHeightOffset;

    private int middleRow;
    private int middleColumn;

    private float moveSpeed;

    private int curStartIndex;
    private int curEndIndex;

    private bool isMoveFinished = false;


    private void InitConfig() {
        this.centerRow = m_Row / 2;
        this.centerColumn = m_Column / 2;

        //中心4行
        this.startRow = m_Row % 2 == 0 ? centerRow - 2 : centerRow - 1;
        this.endRow = m_Row % 2 == 0 ? centerRow + 1 : centerRow + 2;
        //中心8列
        this.startColumn = m_DisplayColumn % 2 == 0 ? centerColumn - 3 : centerColumn - 2;
        this.endColumn = m_DisplayColumn % 2 == 0 ? centerColumn + 2 : centerColumn + 3;

        this.maxHeightOffset = 2f * (m_ImageItemHeight + m_ImageSpace);
        this.minHeightOffset = 0.5f * (m_ImageItemHeight + m_ImageSpace);

        this.middleRow = (startRow + endRow) / 2;
        this.middleColumn = (startColumn + endColumn) / 2;

        this.moveSpeed = 10f;

        this.curStartIndex = this.startColumn;
        this.curEndIndex = this.endColumn;
    }

    private void InitPosition() {
        int index = -1;
        bool isSpecial = false;
        for (int i = 0; i < m_Row; i++) {
            for (int j = 0; j < m_Column; j++) {
                index++;
                var width = j * (m_ImageItemWidth + m_ImageSpace);
                var height = -i * (m_ImageItemHeight + m_ImageSpace);
                isSpecial = false;

                if (i >= startRow && i <= middleRow) {
                    if (j >= startColumn && j <= middleColumn) {
                        isSpecial = true;
                        float ratio = (float)(j - startColumn) / (middleColumn - startColumn);
                        height += Mathf.Lerp(minHeightOffset, maxHeightOffset, ratio);
                    } else if (j >= middleColumn && j <= endColumn) {
                        isSpecial = true;
                        float ratio = (float)(j - middleColumn - 1) / (endColumn - middleColumn - 1);
                        height += Mathf.Lerp(maxHeightOffset, minHeightOffset, ratio);
                    }
                } else if (i >= centerRow && i <= endRow) {
                    if (j >= startColumn && j <= middleColumn) {
                        isSpecial = true;
                        float ratio = (float)(j - startColumn) / (middleColumn - startColumn);
                        height -= Mathf.Lerp(minHeightOffset, maxHeightOffset, ratio);
                    } else if (j >= middleColumn && j <= endColumn) {
                        isSpecial = true;
                        float ratio = (float)(j - middleColumn - 1) / (endColumn - middleColumn - 1);
                        height -= Mathf.Lerp(maxHeightOffset, minHeightOffset, ratio);
                    }
                }
                m_ItemDatas.Add(new ItemData() {
                    code = index,
                    row = i,
                    column = j,
                    position = new Vector2(width, height),
                    isSpecial = isSpecial
                });
            }
        }
    }

    bool isFinished = false;

    private void SetImages(List<Texture2D> texture2Ds) {
        if (texture2Ds == null && texture2Ds.Count == 0) return;
        ClearItems();
        int index = 0;
        for (int i = 0; i < m_ItemDatas.Count; i++) {
            if (texture2Ds.Count >= m_ItemDatas.Count) index = i;
            else index = i % texture2Ds.Count;
            CreateImageItem(i, texture2Ds[index]);
        }

        isFinished = true;

        SetItemSibling();
    }

    private void CreateImageItem(int index, Texture2D texture) {
        var curRow = index / m_Column;
        var itemData = m_ItemDatas[index];
        UIImageItem uIImageItem = Instantiate(m_UIImageItem, transform);
        uIImageItem.gameObject.SetActive(true);
        uIImageItem.Init(itemData);
        uIImageItem.SetImage(texture);
        uIImageItem.SetText(index.ToString());
        uIImageItem.SetSize(m_ImageItemWidth, m_ImageItemHeight);
        uIImageItem.SetPosition(new Vector2(Screen.width, -curRow * (m_ImageItemHeight + m_ImageSpace)), itemData.position);
        uIImageItem.OnImageItemClicked = OnItemClicked;
        m_UIImageItems.Add(itemData, uIImageItem);
    }

    private void RefreshItemPosition() {
        if (!isFinished) return;

        SetHorizontalEffect();

        SetSpecialItemEffect();

        SetItemSibling();
    }

    private void SetHorizontalEffect() {
        for (int i = 0; i < m_Row; i++) {
            for (int j = 0; j < m_Column; j++) {
                UIImageItem curItem = GetItem(i, j);
                RectTransform curRect = curItem.GetComponent<RectTransform>();
                //移动X轴
                curRect.Translate(new Vector2(-1, 0) * Time.deltaTime * moveSpeed);
                //当X轴移动到最左边时，对象更新位置到最右边
                if (curRect.anchoredPosition.x <= -curRect.sizeDelta.x) {
                    curRect.anchoredPosition = new Vector2(Screen.width, curItem.OriginPos.y);
                }
            }
        }
    }

    private void SetSpecialItemEffect() {
        isMoveFinished = false;

        for (int i = startRow; i <= endRow; i++) {

            int index = curStartIndex;

            int finishedCount = 0;

            for (int j = startColumn; j <= endColumn + 1; j++) {

                index = index > m_Column - 1 ? 0 : index;

                UIImageItem curItem = GetItem(i, index);
                RectTransform curRect = curItem.GetComponent<RectTransform>();

                curItem.GetComponentInChildren<RawImage>().color = Color.red;

                curItem.transform.SetAsLastSibling();

                Vector2 targetPos = GetTargetPosition(i, j - 1);
                Vector3 dir = Vector3.Normalize(targetPos - curRect.anchoredPosition);
                curRect.Translate(new Vector2(0, dir.y) * moveSpeed * Time.deltaTime);

                if (Vector3.Distance(curRect.anchoredPosition, targetPos) <= 0.1f) {

                    curRect.anchoredPosition = targetPos;

                    isMoveFinished = true;

                    finishedCount++;
                }

                index++;
            }

            if (isMoveFinished && i == endRow) {
                curStartIndex++;

                curStartIndex = curStartIndex > m_Column - 1 ? 0 : curStartIndex;

            }
        }
    }

    private void SetItemSibling() {
        foreach (var item in m_UIImageItems) {
            if (item.Key.isSpecial)
                item.Value.transform.SetAsLastSibling();
        }
    }

    private void ClearItems() {
        foreach (var item in m_UIImageItems) {
            Destroy(item.Value.gameObject);
        }
        m_UIImageItems.Clear();
    }

    private UIImageItem GetItem(int row, int column) {
        return m_UIImageItems.Where(k => k.Key.row == row && k.Key.column == column)
                             .Select(v => v.Value).FirstOrDefault();
    }

    private Vector3 GetTargetPosition(int row, int column) {
        return m_ItemDatas.Where(k => k.row == row && k.column == column)
                          .Select(k => k.position).FirstOrDefault();
    }

    private void CreateCenterDetail(Texture2D texture) {
        if (m_UICenterDetial == null)
            m_UICenterDetial = Instantiate(Resources.Load<UICenterDetail>("UICenterDetail"), transform.parent);
        m_UICenterDetial.gameObject.SetActive(true);
        m_UICenterDetial.SetDetailContent(texture);
        m_UICenterDetial.OnCloseBtnClicked = () => {
            ResetShowDetailEffect();
        };
    }

    private void CreateItemDetail(Texture2D texture) {
        if (m_UIItemDetail == null)
            m_UIItemDetail = Instantiate(Resources.Load<UIItemDetail>("UIItemDetail"), transform.parent);
        m_UIItemDetail.gameObject.SetActive(true);
        m_UIItemDetail.SetDetailContent(texture);
        m_UIItemDetail.OnCloseBtnClicked = () => { ResetShowDetailEffect(); };
    }

    private void SetShowDetailEffect() {

    }

    private void ResetShowDetailEffect() {

    }

    private void SetDetailImage() {
        string path = "C:/Users/imtect/Desktop/Detail";
        StartCoroutine(LocalResCacheManager.Instance.LoadTextures(path, texs => {
            SetShowDetailEffect();
            CreateItemDetail(texs.Values.ToList()[UnityEngine.Random.Range(0, texs.Values.Count)]);
        }));
    }

    #endregion

    #region Event
    private void OnItemClicked(ItemData data) {
        if (data == null) return;
        UIImageItem uIImageItem = m_UIImageItems[data];

        if (uIImageItem != null) {
            //显示详情
            SetDetailImage();
        }
    }
    #endregion
}

public class ItemData {
    public int code;
    public int row;
    public int column;
    public Vector2 position;
    public bool isSpecial;
}