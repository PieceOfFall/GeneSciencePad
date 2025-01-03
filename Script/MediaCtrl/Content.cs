using UnityEngine;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    public GameObject content;

    private Image btnImg;
    public Sprite defaultSprite;
    public Sprite activeSprite;
    

    private void Awake()
    {
        btnImg = GetComponent<Image>();
    }

    public void Active()
    {
        btnImg.sprite = activeSprite;
        content.SetActive(true);
    }

    public void Inactive()
    {
        btnImg.sprite = defaultSprite;
        content.SetActive(false);
    }
}
