using UnityEngine;

public class MonitorSwitch : MonoBehaviour
{
    public GameObject screen;
    public GameObject screen2;
    public GameObject monitor;
    public void switchScreen(bool switchValue)
    {
        screen.SetActive(!switchValue);
        screen2.SetActive(!switchValue);
    }

    public void switchMonitor(bool switchValue)
    {
        monitor.SetActive(switchValue);
    }

    public void toggleScreen()
    {
        screen.SetActive(true);
        screen2.SetActive(true);

        monitor.SetActive(false);
    }

    public void toggleMonitor()
    {
        monitor.SetActive(true);

        screen.SetActive(false);
        screen2.SetActive(false);
    }
}
