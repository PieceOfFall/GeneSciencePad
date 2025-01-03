using MQTTnet;
using UnityEngine;

public class MQTTPublisher : MonoBehaviour
{
    public string PublishTopic;
    public MQTT Mqtt;

    public void Publish(string msg) => Mqtt.Publish(PublishTopic, msg);
}
