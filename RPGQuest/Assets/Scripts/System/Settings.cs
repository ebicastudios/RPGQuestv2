using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Settings : MonoBehaviour {

    [Header("Debug Flags")]
    public bool logging = false;

    [Header("KeyMap")]
    public string keyUp;
    public string keyLeft;
    public string keyRight;
    public string keyDown;

    public string save()
    {
        string retString = "";                              // Declare the return string
        retString += "keyUp=<" + keyUp + ">;\n";            // Populate the return string with the required fields
        retString += "keyLeft=<" + keyLeft + ">;\n";
        retString += "keyRight=<" + keyRight + ">;\n";
        retString += "keyDown=<" + keyDown + ">;\n";
        if (logging)
        {
            Debug.Log("Settings: I'm sending the following string to SaveLoad:\n" + retString);
        }
        return retString;                                   // Return the retString
    }

    public void load()
    {
        string readIn = SaveLoad.loadSettings();
        keyUp = Parser.getFieldString(readIn, "keyUp");
        keyDown = Parser.getFieldString(readIn, "keyDown");
        keyLeft = Parser.getFieldString(readIn, "keyLeft");
        keyRight = Parser.getFieldString(readIn, "keyRight");
    }
}
