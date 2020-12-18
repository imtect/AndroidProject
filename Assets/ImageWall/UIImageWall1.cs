using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BOE.ResouseMng.LocalRes;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class ItemData {
    public int code;
    public int row;
    public int column;
    public Vector2 position;
    public bool isSpecial;
    public Vector2 targetPositon;
}

public class UIImageWall1 : MonoBehaviour {

    #region 变量

    private float m_Width;
    private float m_Height;

    private int m_Row;
    private int m_DisplayColumn; //显示的列数
    private int m_Column; //m_DisplayColumn + 1
    private float m_ImageSpace;

    private float m_ImageItemWidth;
    private float m_ImageItemHeight;

    private UIImageItem1 m_UIImageItem;
    private Transform m_CenterImage;

    private List<ItemData> m_ItemDatas = new List<ItemData>();
    private Dictionary<ItemData, UIImageItem1> m_UIImageItems = new Dictionary<ItemData, UIImageItem1>();

    private void Awake() {
        this.m_Width = Screen.width;
        this.m_Height = Screen.height;

        m_UIImageItem = transform.Find("ImageItem").GetComponent<UIImageItem1>();
        if (m_UIImageItem) m_UIImageItem.gameObject.SetActive(false);

        m_CenterImage = transform.Find("CenterImage");
        if (m_CenterImage) m_CenterImage.gameObject.SetActive(false);
    }

    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.Space)) {
            RefreshItemPosition();
        }
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
                //显示中心详情页
            });
        });
    }

    public void SetDetailImage(string path) {
        if (string.IsNullOrEmpty(path)) return;
        LocalResCacheManager.Instance.LoadTexture(path, (texs, str) => {

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

    private int curEndColumn;

    private float moveSpeed;

    private int curStartIndex;
    private int curEndIndex;

    private void InitConfig() {
        this.centerRow = m_Row / 2;
        this.centerColumn = m_Column / 2;

        //中心4行
        this.startRow = m_Row % 2 == 0 ? centerRow - 2 : centerRow - 1;
        this.endRow = m_Row % 2 == 0 ? centerRow + 1 : centerRow + 2;
        //中心8列
        this.startColumn = m_DisplayColumn % 2 == 0 ? centerColumn - 4 : centerColumn - 3;
        this.endColumn = m_DisplayColumn % 2 == 0 ? centerColumn + 3 : centerColumn + 4;

        this.maxHeightOffset = 1.5f * (m_ImageItemHeight + m_ImageSpace);
        this.minHeightOffset = 0.2f * (m_ImageItemHeight + m_ImageSpace);

        this.middleRow = (startRow + endRow) / 2;
        this.middleColumn = (startColumn + endColumn) / 2;

        this.curEndColumn = this.endColumn;

        this.moveSpeed = 100f;

        this.curStartIndex = this.startColumn - 1;
        this.curEndIndex = this.endColumn - 1;
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
                    isSpecial = isSpecial,
                    targetPositon = new Vector2(width, height)
                }); ;
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

        RefreshItemSibling();
    }

    private void CreateImageItem(int index, Texture2D texture) {
        var curRow = index / m_Column;
        var itemData = m_ItemDatas[index];
        UIImageItem1 uIImageItem = Instantiate(m_UIImageItem, transform);
        uIImageItem.gameObject.SetActive(true);
        uIImageItem.Init(itemData);
        //uIImageItem.SetImage(texture);
        uIImageItem.SetText(index.ToString());
        uIImageItem.SetSize(m_ImageItemWidth, m_ImageItemHeight);
        uIImageItem.SetPosition(new Vector2(Screen.width, -curRow * (m_ImageItemHeight + m_ImageSpace)), itemData.position);
        uIImageItem.OnImageItemClicked = OnItemClicked;
        m_UIImageItems.Add(itemData, uIImageItem);
    }

    private int ddd = 0;

    private void RefreshItemPosition() {
        if (!isFinished) return;

        for (int i = 0; i < m_Row; i++) {
            for (int j = 0; j < m_Column; j++) {
                UIImageItem1 curItem = GetItem(i, j);
                RectTransform curRect = curItem.GetComponent<RectTransform>();
                //移动X轴
                curRect.Translate(new Vector2(-1, 0) * Time.deltaTime * moveSpeed);
                //当X轴移动到最左边时，对象更新位置到最右边
                if (curRect.anchoredPosition.x < -curRect.sizeDelta.x) {
                    curRect.anchoredPosition = new Vector2(Screen.width, curItem.OriginPos.y);
                }
            }
        }

        for (int i = startRow; i <= endRow; i++) {
            for (int j = startColumn; j <= endColumn + 1; j++) {

                UIImageItem1 curItem = GetItem(i, j);
                RectTransform curRect = curItem.GetComponent<RectTransform>();
                ItemData curData = curItem.ItemData;

                curItem.GetComponentInChildren<RawImage>().color = Color.red;

                Vector2 targetPos = GetTargetPosition(i, j - 1);
                Vector3 dir = Vector3.Normalize(targetPos - curData.position);
                curRect.Translate(new Vector2(0, dir.y) * moveSpeed * Time.deltaTime);

                if (Mathf.Abs(curRect.anchoredPosition.y - targetPos.y) <= 0.01f) {
                    Debug.LogError($"{curData.code} : +++");
                }

            }
        }



        //计算所有的特殊点
        //for (int i = startRow; i <= endRow; i++) {

        //    int index = curStartIndex;

        //    for (int j = startColumn; j <= endColumn + 1; j++) {

        //        //初始索引
        //        index = index > m_Column - 1 ? 0 : index;

        //        //获取当前的Item
        //        UIImageItem1 curItem = GetItem(i, index);
        //        RectTransform curRect = curItem.GetComponent<RectTransform>();
        //        ItemData curData = curItem.ItemData;

        //        //改变颜色
        //        curItem.GetComponentInChildren<RawImage>().color = Color.red;

        //        //获取对应的坐标
        //        Vector2 targetPos = GetTargetPosition(i, j - 1);
        //        Vector3 dir = Vector3.Normalize(targetPos - curData.position);
        //        curRect.Translate(new Vector2(0, dir.y) * moveSpeed * Time.deltaTime);

        //        //计算差值
        //        var offset = Mathf.Abs(curRect.anchoredPosition.y - targetPos.y);

        //        if (offset <= 0.1f) {
        //            //固定位置
        //            curRect.anchoredPosition = targetPos;

        //            if (index == startColumn)
        //                curRect.GetComponentInChildren<RawImage>().color = Color.white;
        //            else if (index == endColumn + 1) {
        //                curRect.GetComponentInChildren<RawImage>().color = Color.red;
        //            }

        //            //更新索引
        //            ddd = curStartIndex + 1;
        //        }

        //        index++;
        //    }
        //}

        ////更新初始索引
        //curStartIndex = ddd > m_Column ? 0 : ddd;

        RefreshItemSibling();
    }

    private void RefreshItemSibling() {
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

    private UIImageItem1 GetItem(int row, int column) {
        return m_UIImageItems.Where(k => k.Key.row == row && k.Key.column == column)
                             .Select(v => v.Value).FirstOrDefault();
    }

    private Vector3 GetTargetPosition(int row, int column) {
        return m_ItemDatas.Where(k => k.row == row && k.column == column)
                          .Select(k => k.position).FirstOrDefault();
    }
    #endregion

    #region Event
    private void OnItemClicked(ItemData data) {
        if (data == null) return;
        UIImageItem1 uIImageItem = m_UIImageItems[data];

        if (uIImageItem != null) {
            //显示详情
        }
    }
    #endregion
}

