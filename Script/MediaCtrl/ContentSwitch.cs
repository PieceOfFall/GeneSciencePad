using UnityEngine;

public class ContentSwitch : MonoBehaviour
{
    public Content defaultActiveBtn;
    private Content currentActive;

    private void Start() => ActiveContent(defaultActiveBtn);

    public void ActiveContent(Content contentBtn)
    {
        if(currentActive == contentBtn) return;
        contentBtn.Active();
        currentActive?.Inactive();
        currentActive = contentBtn;
    }
}
