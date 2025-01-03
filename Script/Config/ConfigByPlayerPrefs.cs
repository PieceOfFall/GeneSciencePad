using System.Collections.Generic;
using UnityEngine;

public class ConfigByPlayerPrefs : MonoBehaviour
{

    public List<ConfigItem> configItems;

    private void Start()
    {
        foreach (var item in configItems)
        {
            item.inputField.text = PlayerPrefs.GetString(item.key, item.defaultValue);
        }
    }

    public void SaveConfig()
    {
        foreach (var item in configItems)
        {
            PlayerPrefs.SetString(item.key, item.inputField.text);
        }
    }
}
