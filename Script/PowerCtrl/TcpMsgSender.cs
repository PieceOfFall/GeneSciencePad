
using UnityEngine;

public class TcpMsgSender : MonoBehaviour
{
    public MQTT Mqtt;
    public string Topic;

    public bool IsHex;
    public string Ip;
    public int Port;
    public string Msg;

    public void PublishTcpMsg()
    {
        TcpCtrlMsg tcpCtrlMsg = new() { msg = Msg, ip = Ip, port = Port, isHex = IsHex };
        string msg = Newtonsoft.Json.JsonConvert.SerializeObject(tcpCtrlMsg);

        Mqtt.Publish(Topic, msg.ToLower().Replace(" ",""));
    }
}
