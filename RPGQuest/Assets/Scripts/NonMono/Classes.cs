using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#region Custom Exceptions
public class MalformedFieldException : Exception
{
    public MalformedFieldException()
    {
    }

    public MalformedFieldException(string message)
    {
    }

    public MalformedFieldException(string message, Exception inner)
    {
    }
}             // Custom exception class for malformed fields. Used by the Parser.
public class FieldNotFoundException : Exception
{
    public FieldNotFoundException()
    {
    }

    public FieldNotFoundException(string message)
    {
    }

    public FieldNotFoundException(string message, Exception inner)
    {
    }
}              // Custom exception class thrown when a field is not found. Used by the Parser.
public class InvalidSaveIndexException : Exception
{
    public InvalidSaveIndexException()
    {
    }

    public InvalidSaveIndexException(string message)
    {
    }

    public InvalidSaveIndexException(string message, Exception inner)
    {
    }
}           // Custom exception class thrown when the GameState class member saveIndex is null
#endregion

#region Saveable Classes
public class Character : iSaveable                                        // Implements the iSaveable interface for reading/writing data to disk                                   
{
    #region Variable Declarations
    private bool isEnabled;                                               // Is the character currently enabled in the game (changed by plot progression)

    private string characterName;                                         // Characters name, as displayed in game

    private int currentHP;                                                // Characters current HP
    private int maxHP;                                                    // Characters maximum HP
    private int pAtk;                                                     // Characters physical attack
    private int mAtk;                                                     // Characters magical attack
    private int pDef;                                                     // Characters physical defense
    private int mDef;                                                     // Characters magical defense
    private int dodge;                                                    // Characters dodge rating
    private int concentration;                                            // Characters concentration rating
    private int critRate;                                                 // Characters critical hit rating

    private int fireAlign;                                                // Characters fire elemental alignment
    private int windAlign;                                                // Characters wind elemental alignment
    private int earthAlign;                                               // Characters earth elemental alignment
    private int waterAlign;                                               // Characters water elemental alignment

    private int level;                                                    // Characters current level

    #endregion
  
    #region Constructors
    public Character()
    {
        isEnabled = false;
        characterName = "NULL";
        currentHP = 1;
        maxHP = 2;
        pAtk = 3;
        mAtk = 4;
        pDef = 5;
        mDef = 6;
        dodge = 7;
        concentration = 8;
        critRate = 9;
        fireAlign = 10;
        windAlign = 11;
        earthAlign = 12;
        waterAlign = 13;
        level = 14;
    }                                                 // Default constructor. Initializes the Character to null parameters
    #endregion

    #region Save/Load Methods
    public string save()
    {
        string retVal = "";                                                     // Declare the return value string

        retVal += "isEnabled=<" + isEnabled + ">;\n";                       // Add the bool determining if the character is enabled or not
        retVal += "characterName=<" + characterName + ">;\n";               // Add the character's name
        retVal += "currentHP=<" + currentHP + ">;\n";                       // Add the character's current HP
        retVal += "maxHP=<" + maxHP + ">;\n";                               // Add the character's maximum HP
        retVal += "pAtk=<" + pAtk + ">;\n";                                 // Add the character's physical attack rating
        retVal += "mAtk=<" + mAtk + ">;\n";                                 // Add the character's magical attack rating
        retVal += "pDef=<" + pDef + ">;\n";                                 // Add the character's physical defense rating
        retVal += "mDef=<" + mDef + ">;\n";                                 // Add the character's magical defense rating
        retVal += "dodge=<" + dodge + ">;\n";                               // Add the character's dodge rating
        retVal += "concentration=<" + concentration + ">;\n";               // Add the character's concentration rating
        retVal += "critRate=<" + critRate + ">;\n";                         // Add the character's crit rating
        retVal += "fireAlign=<" + fireAlign + ">;\n";                       // Add the character's fire elemental alignment
        retVal += "windAlign=<" + windAlign + ">;\n";                       // Add the character's wind elemental alignment
        retVal += "earthAlign=<" + earthAlign + ">;\n";                     // Add the character's earth elemental alignment
        retVal += "waterAlign=<" + waterAlign + ">;\n";                     // Add the character's water elemental alignment
        retVal += "level=<" + level + ">;\n";                               // Add the character's level

        return retVal;                                                      // Return the value.
    }                                               // Method for saving the character to disk. Enforced by iSaveable

    public void load(string what)
    {
        try
        {
            isEnabled = Parser.populate<bool>(what, "isEnabled");
            characterName = Parser.populate<string>(what, "characterName");
            currentHP = Parser.populate<int>(what, "currentHP");
            maxHP = Parser.populate<int>(what, "maxHP");
            pAtk = Parser.populate<int>(what, "pAtk");
            mAtk = Parser.populate<int>(what, "mAtk");
            pDef = Parser.populate<int>(what, "pDef");
            mDef = Parser.populate<int>(what, "mDef");
            dodge = Parser.populate<int>(what, "dodge");
            concentration = Parser.populate<int>(what, "concentration");
            critRate = Parser.populate<int>(what, "critRate");
            fireAlign = Parser.populate<int>(what, "fireAlign");
            windAlign = Parser.populate<int>(what, "windAlign");
            waterAlign = Parser.populate<int>(what, "waterAlign");
            earthAlign = Parser.populate<int>(what, "earthAlign");
            level = Parser.populate<int>(what, "level");
        }
        catch
        {
            Debug.Log("ERROR: Character file imporperly loaded.");
        }
    }                                          // Method for loading the character from disk. Enforced by iSaveable. (PARTIAL IMPLEMENT)

    #endregion

    #region Retrieve Private Value Methods
    public string getName()
    {
        return characterName;
    }                                            // Get the character's display name
    public bool getIsEnabled()
    {
        return isEnabled;
    }                                         // Get whether or not the character is enabled
    public int getLevel()
    {
        return level;
    }                                              // Get the character's level
    #endregion
}                           // Class definition for a battle character (player or NPC). Must implement iFileManager for saving/loading to/from disk. (PARTIAL IMPLEMENT) 

public class PreviewFile
{
    //TO-IMPLEMENT
    // Date time structure for display (how much time has passed since the player started the game

    public bool[] isEnabled = new bool[5];                  // Are the characters enabled?
    public int[] levels = new int[5];                       // What's the levels for the characters?
    public int gold;                                        // How much gold does the party have?
    public string location;                                 // What's the current location of the party?

    public bool isCorrupt;                                  // Is the preview file corrupt?
    public bool isSaveEnabled;                              // Is there even any data to start with?

    public string save()
    {
        string retVal = "";                                 // Will hold the return string for saving
        
        for(int i = 0; i < 5; i++)                                      // Iterate through the arrays and construct values
        {
            retVal += i + "isEnabled=<" + isEnabled[i] + ">;\n";        // Add if the character is enabled
            retVal += i + "levels=<" + levels[i] + ">;\n";              // Add the levels of the corresponding characters
        }

        retVal += "gold=<" + gold + ">;";                               // Add how much gold the party has
        retVal += "location=<" + location + ">;";                       // Add the location of the player party
        retVal += "isCorrupt=<" + isCorrupt + ">;";                     // Add if the file is corrupted
        retVal += "isSaveEnabled=<" + isSaveEnabled + ">;";             // Is the save enabled at all?

        return retVal;                                      // Return the value
    }
    
    public void load(string what)
    {
        // TODO
        // Implement Exception Handling
        try
        {
            isEnabled[0] = Parser.populate<bool>(what, "0isEnabled");
            isEnabled[1] = Parser.populate<bool>(what, "1isEnabled");
            isEnabled[2] = Parser.populate<bool>(what, "2isEnabled");
            isEnabled[3] = Parser.populate<bool>(what, "3isEnabled");
            isEnabled[4] = Parser.populate<bool>(what, "4isEnabled");

            levels[0] = Parser.populate<int>(what, "0levels");
            levels[1] = Parser.populate<int>(what, "1levels");
            levels[2] = Parser.populate<int>(what, "2levels");
            levels[3] = Parser.populate<int>(what, "3levels");
            levels[4] = Parser.populate<int>(what, "4levels");

            gold = Parser.populate<int>(what, "gold");
            location = Parser.populate<string>(what, "location");
            isCorrupt = Parser.populate<bool>(what, "isCorrupt");
            isSaveEnabled = Parser.populate<bool>(what, "isSaveEnabled");
        }
        catch
        {
            isCorrupt = true;                                           // Something went wrong when loading. This means the file is corrupted.
            throw new System.Exception("Something happened when loading preview file via method load(string)");
        }


    }

    public PreviewFile()
    {
        for(int i = 0; i < 5; i++)
        {
            isEnabled[i] = false;
            levels[i] = 1;
        }
        gold = 0;
        location = "NULL";
        isCorrupt = false;
        isSaveEnabled = false;
    }
}       // Class definition for the preview data used by the loading screen. (PARTIAL IMPLEMENT)

#endregion

