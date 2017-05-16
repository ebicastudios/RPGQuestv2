using UnityEngine;
using System.Collections;

public enum eSaveIndex
{
    Null = 0,
    First,
    Second,
    Third
}       // Indexes the save data files. Used by GameState
public enum eFileList
{
    VINFO = 0,
    ERRORLOG,
    B1Preview0,
    B1Key,
    B1C,
    B1PData,
    B1GState,
    B1WInfo,
    B2Preview,
    B2Key,
    B2C,
    B2PData,
    B2GState,
    B2WInfo,
    B3Preview,
    B3Key,
    B3C,
    B3PData,
    B3GState,
    B3WInfo,
    MS1Preview,
    MS1Key,
    MS1C,
    MS1PData,
    MS1GState,
    MS1WInfo,
    MS2Preview,
    MS2Key,
    MS2C,
    MS2PData,
    MS2GState,
    MS2WInfo,
    MS3Preview,
    MS3Key,
    MS3C,
    MS3PData,
    MS3GState,
    MS3WInfo


}        // Indexes the various save files. Used by FileManager
public enum eVersionInfo
{
    V0_0_1,                     // Version 0.0.1
    V0_0_2                      // Version 0.0.2
}     // Indexes the different versions of the game. Used by FileManager and various other scripts
public enum eGameState
{
    Init = 0,
    SplashScreen,
    MainiMenu,
    NewGameMenu,
    LoadGameMenu,
    SettingsMenu,
    CreditsScreen,
    Exit
}       // Indexes the different game states. Used by the GameState class to manage logic
public enum eMusic
{
    MainMenuTheme = 0,
}           // Indexes the different music sources for the game. Used by AudioManager static class
public enum eSFX
{
    MoveSelection = 0,
    AcceptSelection
}             // Indexes the different sfx sources for the game. Used by the AudioManager static class