// This script initializes static classes to ensure smooth operation. Once it has completed it's initialization functionality,
// it destroys itself. This script should be attached to the "System" GameObject. Further, the order of initialization IS IMPORTANT.
// The initialization order is as follows:
//
// SettingInfo
// FileManager
// GameState
// Parser (Parser is independent of other classes, so it can be initialized whenever)
//
// Note: MAKE SURE BEFORE FINAL RELEASE that if a static class exists (as defined by Static.cs), it is
// initialized with a call to it's init() method.
// 
// Brandon Bush
// Ebica Studios
// Last Update: 05/08/2017

using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {

    void Awake()
    {
        SettingInfo.init();         // Initializes the SettingInfo, which is global settings (applies to all saves)
        GameState.init();           // Initialize the GameState
        FileManager.init();         // Initialize the FileManager
        Parser.init();              // Initialize the Parser

    }

    void Start()
    {
        GameState.changeState(eGameState.SplashScreen);     // Change the GameState to Splash Screen
        Destroy(this);                                      // Destroy this script, as it is no longer needed
    }
}
