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
    public static bool encrypt;                         // Whether or not to encrypt files written to disk on save/load

    public static void init()
    {
        Debug.Log("Initializing SettingInfo");
        savePathBase = Application.persistentDataPath;  // Basic persistent data path determined by Unity (cross-platform except for web player, but no worries there)
        logging = true;                                 // Set logging to true, so we are logging every debug message (i.e. in developer or debug mode)
        version = eVersionInfo.V0_0_1;                  // Set the current version
        encrypt = false;                                // Set the encrypt value
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
    public static string currentLocationName;                                     // Holds the current location of the player party
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
    #region Implemented
    private static void checkDirectories()
    {
        // First Level Directories
        if (!Directory.Exists(Application.persistentDataPath + "/dat"))              // Check to see if the dat folder exists
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat");
        }

        // Second Level Directories
        if (!Directory.Exists(Application.persistentDataPath + "/dat/sbackup"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sbackup");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/dat/sdata"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sdata");
        }

        // Third Level Directories
        // Save Backup
        if (!Directory.Exists(Application.persistentDataPath + "/dat/sbackup/s1data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sbackup/s1data");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/dat/sbackup/s2data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sbackup/s2data");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/dat/sbackup/s3data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sbackup/s3data");
        }
        // Main Save Data
        if (!Directory.Exists(Application.persistentDataPath + "/dat/sdata/s1data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sdata/s1data");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/dat/sdata/s2data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sdata/s2data");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/dat/sdata/s3data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/dat/sdata/s3data");
        }

    }                       // Checks to make sure the save directories are present. If not, or if damaged, it fixes them (IMPLEMENTED)
    #endregion

    #region Partial Implement
    public static void makeSaveFile(eSaveIndex which)
    {
        // Determine which files we should write to
        eFileList cFile = new eFileList();                          // Holds which character file we will write to
        eFileList previewFile = new eFileList();                    // Holds which preview file we will write to
        eFileList pFile = new eFileList();                          // Holds which party info file we will write to
        eFileList gFile = new eFileList();                          // Holds which GameState file we will write to
        eFileList wInfo = new eFileList();                          // Holds which WorldInfo file we will write to

        switch (which)                                              // Determine which files to write too
        {
            case eSaveIndex.First:                                  // First save file
                cFile = eFileList.MS1C;
                previewFile = eFileList.MS1Preview;
                pFile = eFileList.MS1PData;
                gFile = eFileList.MS1GState;
                wInfo = eFileList.MS1WInfo;
                break;
            case eSaveIndex.Second:                                 // Second save file
                cFile = eFileList.MS2C;
                previewFile = eFileList.MS2Preview;
                pFile = eFileList.MS2PData;
                gFile = eFileList.MS2GState;
                wInfo = eFileList.MS2WInfo;
                break;
            case eSaveIndex.Third:                                  // Third save file
                cFile = eFileList.MS3C;
                previewFile = eFileList.MS3Preview;
                pFile = eFileList.MS3PData;
                gFile = eFileList.MS3GState;
                wInfo = eFileList.MS3WInfo;
                break;
        }

        // Since the PreviewFile holds data from all other files, we will go ahead and create it here, and update as we go
        PreviewFile preview = new PreviewFile();                    // Holds the preview data

        // Construct the character data
        string cData = "";                                          // String that we will write to file
        List<Character> characters = new List<Character>();         // New list of characters to hold the data
        for(int i = 0; i < 5; i++)                                  // Create the five base characters
        {
            Character newCharacter = new Character();               // Construct a new character
            characters.Add(newCharacter);                           // Store the character
        }

        int cIndex = 0;                                             // Index for the characters when writing data blocks
        foreach(Character c in characters)
        {
            cData += "[CHARACTER" + cIndex + "]";                   // i-th Character Block
            cData += c.save();                                      // Get the save data for the character
            cData += "[END_CHARACTER" + cIndex + "]";               // End this character block
            cIndex++;                                               // Increment the index value
        }

        // Update the preview file with character info
        cIndex = 0;
        foreach(Character c in characters)                          // Iterate through the characters, and use their data to update the preview file
        {
            preview.isEnabled[cIndex] = c.getIsEnabled();           // Get if the character is enabled
            preview.levels[cIndex] = c.getLevel();                  // Get the level of the character
            cIndex++;                                               // Increment the index value
        }

        // Next, construct the party info data
        string pData = "";                                          // Will hold the party data for the pdata files
        PartyInfo.init();                                           // Initialize the PartyInfo to default values
        pData += "gold=<" + PartyInfo.gold + ">;";                  // Construct the gold held by the party

        // Update the preview file
        preview.gold = PartyInfo.gold;                              // Get the gold from PartyInfo

        // Construct the GameState data
        string gData = "";                                          // Will hold the output data for the gstate file
        gData += "currentLocationName=<";                           // Field identifier
        gData += GameState.currentLocationName;                     // Current location display value
        gData += ">;";                                              // End field

        // Update the preview file
        preview.location = GameState.currentLocationName;           // Update with the current location name

        // This is the final update needed to the preview data
        preview.isCorrupt = false;                                  // Since we are constructing the fresh data, it is not corrupt
        preview.isSaveEnabled = false;                              // This save has not been used yet

        // Construct the world info data
        string wData = "";                                          // Holds the world info

        // Construct the preview file data
        string previewData = preview.save();                        // Get the data

        // Finally, send the files along to the writeFile() method to finish up
        writeFile(cFile, cData);
        writeFile(previewFile, previewData);
        writeFile(pFile, pData);
        writeFile(gFile, gData);
        writeFile(wInfo, wData);

        return;                                                     // We're done!
    }
    #endregion

    #region Not Implemented
    #endregion

    #region Public Methods
    public static bool init()
    {
        if (SettingInfo.logging) { Debug.Log("Initializing FileManager"); }
        checkDirectories();                            // Check the directory structure
        

        return true;
    }                                     // Initialization Functionality (PARTIAL IMPLEMENT)
    public static void writeFile(eFileList where, string what)
    {
        // To Implement
        // Finish the switch statement to include all of the eFileList enums

        string filepath = SettingInfo.savePathBase;                  // Get the base save path from SettingInfo
        switch (where)                                               // Determine the filepath
        {
            #region Main Save Files (MS1$)
            case eFileList.MS1Preview:                               // Save 1 preview.dat
                filepath += "/dat/sdata/s1data/preview.dat";
                break;
            case eFileList.MS2Preview:                               // Save 2 preview.dat
                filepath += "/dat/sdata/s2data/preview.dat";
                break;
            case eFileList.MS3Preview:                               // Save 3 preview.dat
                filepath += "/dat/sdata/s3data/preview.dat";
                break;
            case eFileList.MS1C:                                     // Save 1 c.dat
                filepath += "/dat/sdata/s1data/c.dat";
                break;
            case eFileList.MS2C:                                     // Save 2 c.dat
                filepath += "/dat/sdata/s2data/c.dat";
                break;
            case eFileList.MS3C:                                     // Save 3 c.dat
                filepath += "/dat/sdata/s3data/c.dat";          
                break;
            case eFileList.MS1PData:                                 // Save 1 p.dat
                filepath += "/dat/sdata/s1data/pdata.dat";
                break;
            case eFileList.MS2PData:                                 // Save 2 p.dat
                filepath += "/dat/sdata/s2data/pdata.dat"; 
                break;
            case eFileList.MS3PData:                                 // Save 3 p.dat
                filepath += "/dat/sdata/s3data/pdata.dat";
                break;
            case eFileList.MS1GState:                                // Save 1 gstate.dat
                filepath += "/dat/sdata/s1data/gstate.dat";          
                break;
            case eFileList.MS2GState:                                // Save 2 gstate.dat
                filepath += "/dat/sdata/s2data/gstate.dat";
                break;
            case eFileList.MS3GState:                                // Save 3 gstate.dat
                filepath += "/dat/sdata/s3data/gstate.dat";
                break;
            case eFileList.MS1WInfo:                                 // Save 1 winfo.dat
                filepath += "/dat/sdata/s1data/winfo.dat";
                break;
            case eFileList.MS2WInfo:                                 // Save 2 winfo.dat
                filepath += "/dat/sdata/s2data/winfo.dat";
                break;
            case eFileList.MS3WInfo:                                 // Save 3 winfo.dat
                filepath += "/dat/sdata/s3data/winfo.dat";
                break;
            #endregion
        }

        if (SettingInfo.encrypt)                                     // Encrypt the outgoing file
        {
            what = Parser.cryptString(what);                         // Tell the parser to encrypt the string
        }

        using (StreamWriter sw = File.CreateText(filepath))          // Open the streamwriter
        {
            sw.WriteLine(what);                                      // Write the info out
        }

        return;
    }    // Write file specified by "where" with the data "what" (PARTIAL IMPLEMENT)
    public static string readFile(eFileList where)
    {
        // To-Implement
        // Switch Statement: Add all filepaths for all files and shit and stuff

        string retVal = "";                                          // Holds the return data
        string filepath = SettingInfo.savePathBase;                  // Grab the base save path from Setting Info

        switch (where)
        {
            case eFileList.MS1Preview:                               // Main Save 1 preview.dat
                filepath += "/dat/sdata/s1data/preview.dat";        
                break;
            case eFileList.MS2Preview:                               // Main Save 2 preview.dat
                filepath += "/dat/sdata/s2data/preview.dat";
                break;
            case eFileList.MS3Preview:                               // Main Save 3 preview.dat
                filepath += "/dat/sdata/s3data/preview.dat";
                break;
        }

        using (StreamReader sr = File.OpenText(filepath))            // Read in the specified file
        {
            retVal = sr.ReadToEnd();
        }

        if (SettingInfo.encrypt)                                     // Decrypt the string, if necessary
        {
            retVal = Parser.cryptString(retVal);
        }

        return retVal;                                               // Return the data to calling methojd
    }                // Read file specified by "where" (PARTIAL IMPLEMENT)
    #endregion

}             // Static class that handles all disk operations outside of Saving and Loading objects. This includes error correction, backups, etc... (PARTIAL IMPLEMENT)

public static class PartyInfo
{
    public static int gold;                    // Holds how much gold the party has

    public static void init()
    {
        gold = 100;                             // Set the gold in the party's inventory to 100
    }               // Initialize the PartyInfo
}   // Class definition for the data structure holding party information

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
    public static void fadeOut()
    {
        audioCont.fadeOutMusic();
    }
}            // Static class handling loading and playing of music and sound AudioClips. (PARTIAL IMPLEMENT)