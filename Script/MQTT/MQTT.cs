using MQTTnet.Client;
using UnityEngine;
using MQTTnet;
using System.Collections.Generic;

public class MQTT : MonoBehaviour
{
    [HideInInspector]
    public MqttFactory Factory;
    [HideInInspector]
    public IMqttClient Client;

    public string IP = "127.0.0.1";

    public int Port = 1883;

    public List<MQTTMsgSubscribers> msgSubscribers;

    void Start()
    {
        IP = PlayerPrefs.GetString("ip", IP);
        Port = int.Parse(PlayerPrefs.GetString("port", Port.ToString()));
        Factory = MQTTStore.mqttFactory ?? new();
        Client = MQTTStore.mqttClient ?? Factory.CreateMqttClient();

        if (MQTTStore.mqttClient == null)
        {
            InitMqtt();
        }
        else
        {
            msgSubscribers.ForEach(handler => handler.Init());
        }

        MQTTStore.mqttFactory = Factory;
        MQTTStore.mqttClient = Client;
    }

    private async void InitMqtt()
    {
        MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
               .WithTcpServer(IP, Port)
               .Build();

        Client.DisconnectedAsync += async (e) =>
        {
            MqttClientConnectResult reconnectRet = await Client.ConnectAsync(mqttClientOptions);
            if (reconnectRet.ResultCode == MqttClientConnectResultCode.Success)
            {
                msgSubscribers.ForEach(subscriber =>
                {
                    subscriber.OnDestroy();
                    subscriber.Init();
                });
            }
        };

        MqttClientConnectResult ret = await Client.ConnectAsync(mqttClientOptions);
        if (ret.ResultCode != MqttClientConnectResultCode.Success)
        {
            Debug.LogError($"MQTT连接失败: {ret}");
        }
        else
        {
            msgSubscribers.ForEach(handler => handler.Init());
        }
        Debug.Log("MQTT连接成功");
    }


    public async void Publish(string topic, string payload)
    {
        var ret = await Client.PublishAsync(new MqttApplicationMessageBuilder()
           .WithTopic(topic)
           .WithPayload(payload)
           .Build());

        if (!ret.IsSuccess)
            Debug.LogError(ret.ReasonString);
        else Debug.Log($"Send {topic}:\n{payload}");
    }
}
