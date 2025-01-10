using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DeviceCtrl : MonoBehaviour
{

    public MQTT Mqtt;
    public async void PublishDeviceCtrl(bool isPoweron)
    {
        Mqtt.Publish("pc", isPoweron? "allDevice:pcPoweron" : "allDevice:pcPoweroff");

        await Task.Delay(5000);

        Mqtt.Publish("projector",isPoweron? "projector:projectorPoweron": "projector:projectorPoweroff");
    }

}
