using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateSubscriber : MQTTMsgSubscribers
{
    public UnityEvent<State> events;

    public override void Cb(string topic, string msg)
    {
        State state = Newtonsoft.Json.JsonConvert.DeserializeObject<State>(msg);
        events.Invoke(state);
    }
}
