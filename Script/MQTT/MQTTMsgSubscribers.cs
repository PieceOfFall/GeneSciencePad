using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MQTTMsgSubscribers : MonoBehaviour
{
    public string SubscribeTopic;

    public abstract void Cb(string topic, string msg);

    private readonly Queue<Action> uiActionQueue = new();

    public Task AsyncCb(MqttApplicationMessageReceivedEventArgs e)
    {
        string msg = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
        string topic = e.ApplicationMessage.Topic;
        uiActionQueue.Enqueue(() => Cb(topic, msg));
        return Task.CompletedTask;
    }

    /// <summary>
    /// ��ʼ����Ϣ������
    /// 1.������ж���
    /// 2.�����Ϣ�ص�
    /// </summary>
    public async void Init()
    {
        var client = MQTTStore.mqttClient;
        var factory = MQTTStore.mqttFactory;

        client.ApplicationMessageReceivedAsync += AsyncCb;

        MqttClientSubscribeOptionsBuilder subscribeOptionsBuilder = factory.CreateSubscribeOptionsBuilder();
        subscribeOptionsBuilder = subscribeOptionsBuilder.WithTopicFilter(SubscribeTopic);
        MqttClientSubscribeOptions subscribeOptions = subscribeOptionsBuilder.Build();
        await client.SubscribeAsync(subscribeOptions, CancellationToken.None);
    }

    /// <summary>
    /// ����ʱж����Ϣ������
    /// 1. ȡ�����ж���
    /// 2. �Ƴ�������Ϣ�ص�
    /// </summary>
    public async void OnDestroy()
    {
        var client = MQTTStore.mqttClient;
        var factory = MQTTStore.mqttFactory;

        client.ApplicationMessageReceivedAsync -= AsyncCb;

        MqttClientUnsubscribeOptionsBuilder unsubscribeOptionsBuilder = factory.CreateUnsubscribeOptionsBuilder();
        unsubscribeOptionsBuilder = unsubscribeOptionsBuilder.WithTopicFilter(SubscribeTopic);
        MqttClientUnsubscribeOptions unsubscribeOptions = unsubscribeOptionsBuilder.Build();
        await client.UnsubscribeAsync(unsubscribeOptions, CancellationToken.None);
    }

    private void Update()
    {
        while (uiActionQueue.Count > 0)
            uiActionQueue.Dequeue().Invoke();
    }
}
