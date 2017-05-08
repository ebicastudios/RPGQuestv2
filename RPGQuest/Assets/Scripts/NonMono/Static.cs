// These are the static classes used by the game. Static classes are not "instantiated"
// in the classic sense of OOP. Rather, only one copy is instantiated, and no more are
// allowed to exist during the current operation session (i.e. they are singletons)

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class SettingInfo
{
    public static string savePathBase;                  // Base path for saving data
    public static bool logging;                         // Used to determine whether or not to write something to the console log
    public static eVersionInfo version;                 // Holds the current version information for the game

    public static void init()
    {
        Debug.Log("Initializing SettingInfo");
        savePathBase = Application.persistentDataPath;  // Basic persistent data path determined by Unity (cross-platform except for web player, but no worries there)
        logging = true;                                 // Set logging to true, so we are logging every debug message (i.e. in developer or debug mode)
        version = eVersionInfo.V0_0_1;                  // Set the current version
        if (logging)
        {
            Debug.Log("SettingInfo.logging set to true. Debug messages will be logged");
        }
        else
        {
            Debug.Log("SettingInfo.logging set to false. This will be the last normal debug message");
        }
    }
}             // Static Class holding global setting information, keybinds, etc... (PARTIAL IMPLEMENT)

public static class Parser
{
    #region Variables
    private static int privateKey = ((040490 % 031595) + (041064 % 032164)) % 17;          // Private key. DO NOT ALLOW THIS KEY TO EVER BE LEAKED. PERIOD. 
    #endregion

    #region Methods
    public static bool init()
    {
        if (SettingInfo.logging) { Debug.Log("Initializing Parser"); }
        return true;
        // Currently, no initialization is needed. We will add it here if the need arises later
    }                                                           // Initializes the Parser. Currently not needed, so no implementation necessary
    public static T populate<T>(string from, string what)
    {
     
        T retVal;
        int startIndex = 0;                                                     // Used to determine wherethe beginning of the field desired is
        int endIndex = 0;                                                       // Used to determine where the end of the field desired is

        startIndex = from.IndexOf(what);                                        // Finds the index of the first letter of the desired field string

        if (startIndex == -1)                                                   // Check to see if the field was found (IndexOf returns -1 if field is not found)
        {
            throw new FieldNotFoundException("Desired Field Not Found");        // Field was not found, throw a FieldNotFoundException
        }

        startIndex += what.Length;                                              // Move the start index to the end of the string. This should land it on the = character
        startIndex += 2;                                                        // Move the start index to the first character of the desired field

        endIndex = from.IndexOf('>', startIndex);                               // Find the index of the closing bracket '>'. Note that this lands the index before '<', so no -- is needed.

        if (endIndex == -1)                                                     // Check to see if the end of the field was found (IndexOf returns -1 if field is not found)
        {
            throw new MalformedFieldException("Desired Field is Malformed");    // Field is malformed. Throw MalformedFieldException
        }

        string readVal = from.Substring(startIndex, endIndex - startIndex);     // Get a substring of the master string with the field desired

        retVal = (T)Convert.ChangeType(readVal, typeof(T));                     // Convert readVal from type string to type T and store it in retVal

        return retVal;                                                          // Return the field value to the calling method
    }                               // Generic method to return a field as a specified type T
    public static string cryptString(string what)
    {
        string retVal = "";                                 // Holds the return string
        for(int i = 0; i < what.Length; i++)                // Encrypt what and store it into retVal
        {
            char c = what[i];                               // Create a new character c, and store the character at the i-th position of what in it
            int cAsInt = (int)c;                            // Case the Character as an int
            int cEncrypt = cAsInt ^ privateKey;             // XOR the c (as int) with the private key
            retVal += (char)cEncrypt;                       // Store the value in the return string
        }
        return retVal;
    }                                       // Encrypts or decrypts the passed string using XOR encryption.
    #endregion
}                  // Custom built parser for getting fields from save data (PARTIAL IMPLEMENT)

public static class GameState
{
    #region Observer Pattern Logic
    private static List<GameObject> registeredObjects;                            // Holds game objects to be messaged when the state changes
    public static void register(GameObject go)
    {
        if (SettingInfo.logging)
        {
            Debug.Log("Registering object " + go.name + " with GameState");
        }
        if (!registeredObjects.Contains(go))                                // If registeredObjects does not contain go
        {
            registeredObjects.Add(go);                                      // Add go
        }
    }                                 // Registers an the GameObject go as an observer
    public static void deregister(GameObject go)
    {
        if (registeredObjects.Contains(go))                     // If registeredObjects contains go
        {
            registeredObjects.Remove(go);                       // Remove go
        }

    }                               // Removes the GameObject go as an observer
    public static void changeState(eGameState which)
    {
        if (SettingInfo.logging)
        {
            Debug.Log("Changing Game State to " + which.ToString());
        }
        for(int i=0;i<registeredObjects.Count;i++)                              // Iterate through the registered observers
        {
            registeredObjects[i].SendMessage("checkState", which);                             // Send go a message that the gamestate is changing to which
        }
    }                           // Messages registered observers that a state change has occured
    #endregion


    #region Variables
    public static eSaveIndex saveIndex;                                           // Holds the current save index for reading/writing files to disk
    public static List<Character> playerParty;                                    // Holds the character data for the player's party
    #endregion

    #region Initialization Methods
    public static bool init()
    {
        if (SettingInfo.logging) { Debug.Log("Initializing GameState"); }
        
        registeredObjects = new List<GameObject>();
        saveIndex = eSaveIndex.Null;                                              // Initialize the save index to null. If the save index is null, no object will attempt to save itself
        playerParty = new List<Character>();                                      // Initialize the playerParty list
        for(int i = 0; i < 5; i++)
        {
            Character newCharacter = new Character();                             // Create a new instance of a character with the default constructor
            playerParty.Add(newCharacter);                                        // Add the character to the playerParty list
        }
        return true;
    }                                                  // Method to initialize the GameState to a default mode
    #endregion

    #region Save/Load Methods
    public static void save() {

        // Get the data
        string cdata = saveCharacters();        // Get the formatted save data for the player Characters


        // Send Data to the FileManager
        switch (saveIndex)
        {
            case eSaveIndex.First:
                FileManager.writeFile(cdata, eFileList.MS1C);
                break;
            case eSaveIndex.Second:
                FileManager.writeFile(cdata, eFileList.MS2C);
                break;
            case eSaveIndex.Third:
                FileManager.writeFile(cdata, eFileList.MS3C);
                break;
        }
    }                                           // Method to construct save files and send them to the FileManager
    public static void load()
    {

    }                                            // Method to load save files from the FileManager
    private static string saveCharacters()
    {
        string writeOut = "";                                                   // Holds the formatted save string for returning to calling method
        int characterIndex = 0;                                                 // Indexer for the following loop that tells us what character file we're saving
        foreach (Character c in playerParty)                                    // Iterate through the playerParty List<Character> and get the save data from each
        {
            writeOut += "[CHARACTER" + characterIndex + "]\n";                  // Beginning of Data Block
            writeOut += c.save();                                               // Save Data
            writeOut += "[END_CHARACTER" + characterIndex + "]\n";              // End of the Block
            characterIndex++;                                                   // Increment the character indexer
        }

        return writeOut;                                                        // Return formatted save data to calling method

    }                               // Method to construct character save data to send to the FileManager
    private static void loadCharacters(string what)
    {

    }                      // Method to load constructed character save data to send to the FileManager
    #endregion

}               // Static class holding the current GameState. Used by anything executing any form of gameplay logic (PARTIAL IMPLEMENT)

public static class FileManager
{
    #region Public Methods
    public static bool init()
    {
        if (SettingInfo.logging) { Debug.Log("Initializing FileManager"); }
        checkDirectories();                            // Check the directory structure
        checkFiles();                                  // Check the files
        return true;
    }                                     // Initialization Functionality (PARTIAL IMPLEMENT)

    public static string readFile(eFileList which)
    {
        string filePath = SettingInfo.savePathBase;                 // Get the base save path from the SettingInfo (Improves readability)
        string readIn = "";                                         // Holds the return value
        switch (which)                                              // Determine which file to open
        {
            case eFileList.VINFO:                                   // Need to get the vinfo.dat file
                filePath = filePath += "\vinfo.dat";                // Update the filepath to the vinfo.dat location
                break;
        }

        using(StreamReader sr = File.OpenText(filePath))            // Read the specified file in
        {
            readIn = sr.ReadToEnd();
        }

        readIn = Parser.cryptString(readIn);                        // Decrypt the string

        return readIn;                                              // Return the string
    }                // Retrieves a specific file designated by eFileList which (PARTIAL IMPLEMENT)
    public static void writeFile(string what, eFileList which)
    {
        string filepath = SettingInfo.savePathBase;
        switch (which)
        {
            case eFileList.MS1C:                                    // Want to save character data to the first save file
                filepath += "/dat/sdata/s1data/c.dat";              // Update the filepath
                break;
            case eFileList.MS2C:                                    // Want to save character data to the second save file
                filepath += "/dat/sdata/s2data/c.dat";              // Update the filepath
                break;
            case eFileList.MS3C:                                    // Want to save character data to the third save file
                filepath += "/dat/sdata/s3data/c.dat";              // Update the filepath
                break;
        }

        what = Parser.cryptString(what);                            // Encrypt the save data

        using(StreamWriter sw = File.CreateText(filepath))          // Write the data to disk
        {
            sw.WriteLine(what);
        }

    }    // Saves a formatted string "what" into the file specified by eFileList "which" (NOT IMPLEMENTED)

    public static void storeBackups()
    {

    }                             // Store current save data into backup folders (NOT IMPLEMENTED)
    public static void restoreBackup(eSaveIndex which)
    {

    }            // Restore a specific backup save file determined by the value of "which" (NOT IMPLEMENTED)
    #endregion

    #region Private Methods
    private static void checkDirectories()
    {
        // First Level Directories
        if (!Directory.Exists(Application.persistentDataPath + "/dat"))              // Check to see if the dat folder exists
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat");
        }

        // Second Level Directories
        if(!Directory.Exists(Application.persistentDataPath + "/dat/sbackup"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sbackup");
        }
        if(!Directory.Exists(Application.persistentDataPath + "/dat/sdata"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sdata");
        }

        // Third Level Directories
            // Save Backup
        if(!Directory.Exists(Application.persistentDataPath + "/dat/sbackup/s1data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sbackup/s1data");
        }
        if(!Directory.Exists(Application.persistentDataPath + "/dat/sbackup/s2data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sbackup/s2data");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/dat/sbackup/s3data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sbackup/s3data");
        }
            // Main Save Data
        if(!Directory.Exists(Application.persistentDataPath + "/dat/sdata/s1data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sdata/s1data");
        }
        if(!Directory.Exists(Application.persistentDataPath + "/dat/sdata/s2data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sdata/s2data");
        }
        if(!Directory.Exists(Application.persistentDataPath + "/dat/sdata/s3data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sdata/s3data");
        }

    }                       // Checks to make sure the save directories are present. If not, or if damaged, it fixes them (IMPLEMENTED)
    private static void checkFiles()
    {
        string baseFilepath = SettingInfo.savePathBase;                 // Grab the base save path
        // Non-Save Files
        if (!File.Exists(baseFilepath + "/vinfo.dat"))                  // Check the status of vinfo.dat
        {
            createFile(eFileList.VINFO);                                // vinfo.dat does not exist. Let's make it
        }
        if(!File.Exists(baseFilepath + "/error.log"))                   // Check the status of error.log
        {
            createFile(eFileList.ERRORLOG);                             // error.log does not exist. Let's make it
        }
    }                             // Check to see if the some necessary files are present. If not, or if damaged, fixes them (NOT IMPLEMENTED)
    private static void restoreBackup(eFileList which)
    {

    }           // Attempts to restore a backup save file specified by which (NOT IMPLEMENTED)
    private static void createFile(eFileList which)
    {
        switch (which)
        {
            case eFileList.VINFO:                               // Need to create the vinfo.dat file
                makeVInfo();                                    // Call corresponding function
                break;
            case eFileList.ERRORLOG:                            // Need to create error.log file
                makeErrorLog();
                break;                
        }
    }              // Creates a save file specified by which (PARTIAL IMPLEMENT)
    #endregion

    #region File Creation Methods
    private static void makeVInfo()
    {
        string writeOut = "VERSION=<";
        switch (SettingInfo.version)
        {
            case eVersionInfo.V0_0_1:
                writeOut += "V0_0_1";
                break;
            case eVersionInfo.V0_0_2:
                writeOut += "V0_0_2";
                break;
        }
        writeOut = Parser.cryptString(writeOut);
        using (StreamWriter sw = File.CreateText(SettingInfo.savePathBase + "/vinfo.dat"))
        {
            sw.WriteLine(writeOut);
        }
    }                              // Make the vinfo.dat file (PARTIAL IMPLEMENT)
    private static void makeErrorLog()
    {
        string writeOut = "Error Log";
        using(StreamWriter sw = File.CreateText(SettingInfo.savePathBase + "/error.log"))
        {
            sw.WriteLine(writeOut);
        }
    }                           // Make the error.log file (PARTIAL IMPLEMENT)
    #endregion
}             // Static class that handles all disk operations outside of Saving and Loading objects. This includes error correction, backups, etc... (PARTIAL IMPLEMENT)

public static class AudioManager
{
    private static GameObject audioManagerGo;
    private static AudioContainer audioCont;
    private static AudioSource musicSource;
    private static AudioSource sfx1;
    private static AudioSource sfx2;
    private static AudioSource sfx3;
    private static AudioSource sfx4;

    private static float fadeSpeed = .05f;

    public static void register(GameObject go)
    {
        audioManagerGo = go;
        audioCont = go.GetComponent<AudioContainer>();
        musicSource = go.GetComponent<AudioContainer>().musicSource;
        sfx1 = go.GetComponent<AudioContainer>().sfx1;
        sfx2 = go.GetComponent<AudioContainer>().sfx2;
        sfx3 = go.GetComponent<AudioContainer>().sfx3;
        sfx4 = go.GetComponent<AudioContainer>().sfx4;

        sfx1.loop = false;
        sfx2.loop = false;
        sfx3.loop = false;
        sfx4.loop = false;
    }           // Register the AudioManager GameObject and corresponding components with the AudioManager Class (PARTIAL IMPLEMENT)

    public static void playMusic(eMusic which, bool loop)
    {
        musicSource.loop = loop;                            // Set the loop bool to whatever the passed loop is

        switch (which)
        {
            case eMusic.MainMenuTheme:
                musicSource.clip = audioCont.mainMenuTheme;
                break;
        }

        musicSource.Play();
    }   // Play a specified audio clip on the musicSource channel. Loop condition is determined by the passed bool loop.
    public static void playSFX(eSFX which)
    {   
        AudioClip toPlay = null;                            // Holds sfx AudioClip to be played
        switch (which)
        {
            case eSFX.AcceptSelection:                      // Selecting an option on the UI
                toPlay = audioCont.acceptSelection;
                break;
            case eSFX.MoveSelection:                       // Moving the selection on the UI (via Keyboard)
                toPlay = audioCont.moveSelection;
                break;
        }

        // Check the sfx audiosources and use the first one that's free to play the sfx
        if (!sfx1.isPlaying)
        {
            sfx1.clip = toPlay;
            sfx1.Play();
        }
        else if (!sfx2.isPlaying)
        {
            sfx2.clip = toPlay;
            sfx2.Play();
        }
        else if (!sfx3.isPlaying)
        {
            sfx3.clip = toPlay;
            sfx3.Play();
        }
        else if (!sfx4.isPlaying)
        {
            sfx4.clip = toPlay;
            sfx4.Play();
        }
    }
    public static void stopMusic()
    {
        musicSource.Stop();

    }
    public static void fadeIn()
    {
        audioCont.fadeInMusic();
    }
}