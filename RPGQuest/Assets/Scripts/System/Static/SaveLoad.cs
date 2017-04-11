using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

public static class SaveLoad {

    public static bool logging = false;

    private static string settingsPath = Application.persistentDataPath + "/settings.dat";
    private static int key = 040490 % 7701;

    // Use this to save settings files
    public static void save(Settings sets)
    {
        if (logging)
        {
            Debug.Log("SaveLoad: save(Settings) called");
        }

        string saveString = sets.save();

        if (logging)
        {
            Debug.Log("SaveLoad: Got a string to save with contents: " + saveString);
            Debug.Log("SaveLoad: Attempting encryption");
        }
        saveString = encryptDecrypt(saveString);
        if (logging)
        {
            Debug.Log("SaveLoad: Encrypted Settings Save with the following result: \n" + saveString);
        }
        File.WriteAllText(settingsPath, saveString);

        if (logging)
        {
            Debug.Log("SaveLoad: Succesfully wrote settings data");
        }

        return;
    }

    // Use this to load settings files
    public static string loadSettings()
    {
        if (File.Exists(settingsPath))
        {
            if (logging)
            {
                Debug.Log("SaveLoad: Found settings file at " + settingsPath + ". Attempting to load");
            }

            string readIn = File.ReadAllText(settingsPath);
            if (logging)
            {
                Debug.Log("SaveLoad: Loaded in the following string from settings file: \n" + readIn);
                Debug.Log("SaveLoad: Attempting to decrypt file");
            }

            readIn = encryptDecrypt(readIn);

            if (logging)
            {
                Debug.Log("SaveLoad: Decrypted readIn string with the result: \n" + readIn);
            }

            return readIn;

        }

        return "";

    }

    // Encrypt/Decrypt using XOR
    public static string encryptDecrypt(string what)
    {
        StringBuilder inSb = new StringBuilder(what);
        StringBuilder outSb = new StringBuilder(what.Length);
        for (int i = 0; i < what.Length; i++)
        {
            char tempChar;
            tempChar = inSb[i];
            tempChar = (char)(tempChar ^ key);
            outSb.Append(tempChar);
        }
        return outSb.ToString();
    }
}
