using MQTTnet.Client;
using UnityEngine;
using MQTTnet;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
public class MQTT : MonoBehaviour
{
    [HideInInspector]
    public MqttFactory Factory;
    [HideInInspector]
    public IMqttClient Client;


    public string IP = "127.0.0.1";

    public int Port = 1883;

    public List<MQTTMsgSubscribers> msgSubscribers;

    public Image statusImg;
    public Sprite connectSprite;
    public Sprite disconnectSprite;
    private float lastDisconnectTime = 0;
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


    private void OnDestroy()
    {
        Client?.Dispose();
    }

    private void Update()
    {
        if(Client != null)
        {
            statusImg.sprite = Client.IsConnected ? connectSprite : disconnectSprite;

            if (!Client.IsConnected && lastDisconnectTime != 0 && DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - lastDisconnectTime > 20 * 1000f)
            {
                msgSubscribers.ForEach(subscriber =>
                {
                    subscriber.OnDestroy();
                });
                InitMqtt();
                lastDisconnectTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }
    }


    public void resetConnect()
    {
      
            msgSubscribers.ForEach(subscriber =>
            {
                subscriber.OnDestroy();
            });
            InitMqtt();
    }


    private async void InitMqtt()
    {
        MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
               .WithTcpServer(IP, Port)
               .Build();

        Client.DisconnectedAsync += async (e) =>
        {
            lastDisconnectTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Debug.LogError("连接已断开，正在重连");
            MqttClientConnectResult reconnectRet = await Client.ConnectAsync(mqttClientOptions);
            if (reconnectRet.ResultCode == MqttClientConnectResultCode.Success)
            {
                msgSubscribers.ForEach(subscriber =>
                {
                    subscriber.OnDestroy();
                    subscriber.Init();
                });
                Debug.Log("MQTT连接成功");
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
