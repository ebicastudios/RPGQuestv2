using UnityEngine;
using System.Collections;

public class D_SaveLoad : MonoBehaviour {

    [Header("Debug Flags")]
    public bool logging = false;
    public bool enabled = false;

    [Header("What to Do")]
    public bool saveSettings = false;
    public bool loadSettings = false;

    [Header("Required Components")]
    public Settings settings;

    void Awake()
    {
        if (logging)
        {
            Debug.Log("Settings Filepath: " + Application.persistentDataPath);
        }
    }
    void Update()
    {
        if (enabled)
        {
            if (logging)
            {
                settings.logging = true;
                SaveLoad.logging = true;
            }
            else
            {
                settings.logging = false;
                SaveLoad.logging = false;
            }
            if (Input.GetKeyDown("`"))
            {
                if(saveSettings && loadSettings)
                {
                    Debug.Log("D_SaveLoad: WARNING! Both saveSettings and loadSettings flags are set to true. This may cause undefined behavior");
                }
                if (saveSettings)
                {
                    SaveLoad.save(settings);
                }
                else if (loadSettings)
                {
                    settings.load();
                }
            }
        }
    }
}
