using TMPro;
using UnityEngine;

public class PowerCtrlMsgHandler : MQTTMsgSubscribers
{
    public TextMeshProUGUI textMeshPro;
    private int count;
    public override void Cb(string topic,string msg)
    {
        Debug.Log(msg);
        count++;
        string showText = $"accept: {msg}\n,{count}";
        textMeshPro.text = showText;
    }
}
