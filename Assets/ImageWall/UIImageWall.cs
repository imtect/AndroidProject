using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BOE.ResouseMng.LocalRes;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class ItemData {
    public int row;
    public int column;
}

public class UIImageWall : MonoBehaviour {

    private int m_Row;
    private int m_Columne;
    private float m_ImageSpace;

    private float m_Width;
    private float m_Height;

    private float m_ImageItemWidth;
    private float m_ImageItemHeight;

    private int m_TotalCount;

    private UIImageItem m_UIImageItem;

    private Transform m_CenterImage;

    private Dictionary<ItemData, UIImageItem> m_UIImageItems = new Dictionary<ItemData, UIImageItem>();

    private void Awake() {
        this.m_Width = Screen.width;
        this.m_Height = Screen.height;
        m_UIImageItem = transform.Find("ImageItem").GetComponent<UIImageItem>();
        if (m_UIImageItem) m_UIImageItem.gameObject.SetActive(false);

        m_CenterImage = transform.Find("CenterImage");
        m_CenterImage.gameObject.SetActive(false);
    }

    public void SetRowColumn(string data) {
        if (string.IsNullOrEmpty(data)) return;
        string[] sprites = data.Split(',');

        int row = int.Parse(sprites[0]);
        int columne = int.Parse(sprites[1]);
        int imageSpace = int.Parse(sprites[2]);

        this.m_Row = row;
        this.m_Columne = columne;
        this.m_ImageSpace = imageSpace;
        this.m_ImageItemWidth = (m_Width - (m_Columne - 1) * imageSpace) / m_Columne;
        this.m_ImageItemHeight = (m_Height - (m_Row - 1) * imageSpace) / m_Row;
        this.m_TotalCount = row * columne;
    }

    //public void SetRowColumn(int row, int columne, float imageSpace = 0) {
    //    this.m_Row = row;
    //    this.m_Columne = columne;
    //    this.m_ImageSpace = imageSpace;
    //    this.m_ImageItemWidth = (m_Width - (m_Columne - 1) * imageSpace) / m_Columne;
    //    this.m_ImageItemHeight = (m_Height - (m_Row - 1) * imageSpace) / m_Row;
    //    this.m_TotalCount = row * columne;
    //}

    public void SetImages(string dirPath) {
        if (string.IsNullOrEmpty(dirPath)) return;
        //dirPath = Application.persistentDataPath + "/" + dirPath;
        //Debug.Log(Application.persistentDataPath);
        StartCoroutine(LocalResCacheManager.Instance.LoadTextures(dirPath, (texs) => {
            StartCoroutine(SetImages(texs.Values.ToList()));
        }));
    }

    public void SetCenterImages(string centerImagePath) {
        if (string.IsNullOrEmpty(centerImagePath)) return;
        //centerImagePath = Application.persistentDataPath + "/" + centerImagePath;
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

    private void Update() {
       // SetCenterEffect(m_Row / 2, m_Columne / 2);
    }

    private IEnumerator SetImages(List<Texture2D> texture2Ds) {
        if (texture2Ds != null && texture2Ds.Count > 0) {
            ClearItems();
            int index = 0; //图片的索引
            for (int i = 0; i < m_TotalCount; i++) {
                if (texture2Ds.Count >= m_TotalCount) index = i;
                else index = i % texture2Ds.Count;
                CreateImageItem(i, texture2Ds[index]);

                if (i % m_Columne == 0) {//在最右边创建一个过渡位置
                    CreateImageItem(i, texture2Ds[index], false);
                }
            }
        }

        yield return new WaitForSeconds(1f);

        //设置中心区域水滴效果
        SetCenterEffect(m_Row / 2, m_Columne / 2);
    }

    void ClearItems() {
        foreach (var item in m_UIImageItems) {
            Destroy(item.Value.gameObject);
        }
        m_UIImageItems.Clear();
    }

    private void SetCenterEffect(int centerRow, int centerColumn) {
        //中心4行
        int startRow = m_Row % 2 == 0 ? centerRow - 2 : centerRow - 1;
        int endRow = m_Row % 2 == 0 ? centerRow + 1 : centerRow + 2;
        //中心8列
        int startColumn = m_Columne % 2 == 0 ? centerColumn - 4 : centerColumn - 3;
        int endColumn = m_Columne % 2 == 0 ? centerColumn + 3 : centerColumn + 4;

        var maxHeightOffset = 1.5f * (m_ImageItemHeight + m_ImageSpace);
        var minHeightOffset = 0.2f * (m_ImageItemHeight + m_ImageSpace);

        var centRow = (startRow + endRow) / 2;
        var centColumn = (startColumn + endColumn) / 2;

        for (int i = startRow; i <= endRow; i++) {
            for (int j = startColumn; j <= endColumn; j++) {
                UIImageItem uIImageItem = GetItem(i, j);

                uIImageItem.GetComponentInChildren<RawImage>().color = Color.red;

                if (uIImageItem != null) {

                    var initPos = uIImageItem.GetComponent<RectTransform>().anchoredPosition;

                    float height = 0f; 

                    //向上移动
                    Vector2 endPos = Vector2.zero;
                    if (i <= centRow) {
                        if (j <= centColumn) {
                            float ratio = (float)(j - startColumn) / (centColumn - startColumn);
                            height = Mathf.Lerp(minHeightOffset, maxHeightOffset, ratio);
                        } else {
                            float ratio = (float)(j - centColumn - 1) / (endColumn - centColumn - 1);
                            height = Mathf.Lerp(maxHeightOffset, minHeightOffset, ratio);
                        }
                        endPos = new Vector2(initPos.x, initPos.y + height);
                    } else { //向下移动   
                        if (j <= centColumn) {
                            float ratio = (float)(j - startColumn) / (centColumn - startColumn);
                            height = Mathf.Lerp(minHeightOffset, maxHeightOffset, ratio);
                        } else {
                            float ratio = (float)(j - centColumn - 1) / (endColumn - centColumn - 1);
                            height = Mathf.Lerp(maxHeightOffset, minHeightOffset, ratio);
                        }
                        endPos = new Vector2(initPos.x, initPos.y - height);
                    }
                    uIImageItem.SetPosition(initPos, endPos, 0.5f);
                }
            }
        }
    }

    UIImageItem GetItem(int row, int column) {
        return m_UIImageItems.Where(k => k.Key.row == row && k.Key.column == column)
                             .Select(v => v.Value).FirstOrDefault();
    }

    private void CreateImageItem(int index, Texture2D texture, bool isMove = true) {
        var curRow = index / m_Columne;
        var itemData = new ItemData() { row = curRow, column = index - curRow * m_Columne };

        UIImageItem uIImageItem = Instantiate(m_UIImageItem, transform);
        uIImageItem.gameObject.SetActive(true);
        uIImageItem.Init(itemData);
        uIImageItem.SetImage(texture);
        uIImageItem.SetSize(m_ImageItemWidth, m_ImageItemHeight);

        Vector2 initPos = new Vector2(Screen.width, -curRow * (m_ImageItemHeight + m_ImageSpace));
        if (isMove)
            uIImageItem.SetPosition(initPos, GalculatePosition(index, curRow));
        else
            uIImageItem.SetPosition(initPos, initPos);

        uIImageItem.OnImageItemClicked = OnItemClicked;

        m_UIImageItems.Add(itemData, uIImageItem);
    }

    private Vector2 GalculatePosition(int index, int curRow) {
        index = index % m_Columne;
        float width = index * (m_ImageItemWidth + m_ImageSpace);
        float height = -curRow * (m_ImageItemHeight + m_ImageSpace);
        return new Vector2(width, height);
    }

    private void OnItemClicked(ItemData data) {
        if (data == null) return;
        UIImageItem uIImageItem = m_UIImageItems[data];

        if (uIImageItem != null) {
            //显示详情


        }
    }
}

