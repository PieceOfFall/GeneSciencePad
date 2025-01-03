using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwiperItem : MonoBehaviour
{
    public float Pos;

    public GameObject PanelView;

    public Color UnselectColor = new(220f/255f, 220f / 255f, 220f / 255f);

    public Color SelectColor = new(1f, 1f, 1f);

    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SelectItem(bool isSelect)
    {
        textMeshPro.color = isSelect ? SelectColor : UnselectColor;
        PanelView.SetActive(isSelect);
    }

}
