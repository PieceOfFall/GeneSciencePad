using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterSwitch : MonoBehaviour
{
    public GameObject center;
    public GameObject main;

    public void SwitchCenter()
    {
        center.SetActive(true); main.SetActive(false);
    }

    public void SwitchMain()
    {
        center.SetActive(false ); main.SetActive(true);
    }
}
