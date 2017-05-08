using UnityEngine;
using System.Collections;
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

        return retVal;                                                    // Return the value.
    }                                               // Method for saving the character to disk. Enforced by iSaveable

    public void load(string what)
    {

    }                                          // Method for loading the character from disk. Enforced by iSaveable.

    #endregion

    #region Retrieve Private Value Methods
    public string getName()
    {
        return characterName;
    }                                            // Get the character's display name
    #endregion
}                           // Class definition for a battle character (player or NPC). Must implement iFileManager for saving/loading to/from disk.     

#endregion