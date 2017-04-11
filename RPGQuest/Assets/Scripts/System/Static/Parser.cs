using UnityEngine;
using System;                                               // Needed for Int32.TryParse(string, int)
using System.Collections;

public static class Parser {

    private static bool logging = false;

    // This function is used by the debug scripts to set the logging flag. This is to prevent the flag from being changed directly, as it might occur if
    // logging was a public variable
    public static void setLogging(bool what)
    {
        logging = what;
    }

    // Get a field value and return it as a string
    // 
    public static string getFieldString(string what, string field)
    {
        try
        {
            string retVal = getField(what, field);
            return retVal;
        }
        catch (System.Exception e) {
            Debug.Log(e.ToString());
            return "";
        }
    }

    // Get a field value and return it as an int
    //
    public static int getFieldInt(string what, string field)
    {
        try
        {
            int retInt;
            string retString = getField(what, field);
            if(!Int32.TryParse(retString, out retInt))
            {
                throw new System.Exception("Parser: Error parsing string to int :(");
            }
            if (logging)
            {
                Debug.Log("Parser: Successfuly converted string from getField to int with value " + retInt.ToString());
            }
            return retInt;
            
        }
        catch(System.Exception e)
        {
            Debug.Log(e.ToString());
            return -1;
        }
    }
    
    // This function is used as a kind of pseudo-generic function. It will return the field as a string. The calling function (also in
    // this class) will then convert it to the proper type, and return it to the master calling function as required.
    private static string getField(string what, string field)
    {
        if (logging)                                                                // Shows what we are trying to get from the source string in the Console
        {
            Debug.Log("Parser: Finding field " + field + " in string " + what);
        }

        int startIndex = what.IndexOf(field);                                       // Find the index of the first letter of the field string

        if(startIndex == -1)                                                        // The field was not found. This is handled in the calling functions
        {
            throw new System.Exception("Parser: startIndex reports -1 as value. The desired field was not found");
        }

        startIndex += (field.Length + 2);                                           // Adjust the start index to account for the =< characters
        int endIndex = what.IndexOf(">", startIndex);                               // Find the first instance of > after the start index

        if (endIndex == -1)                                                         // The end of the field was not found. Some kind of formatting error
        {
            throw new System.Exception("Parser: endIndex reports -1 as value. End of the field was not found. This is a formatting issue");
        }

        string retVal = what.Substring(startIndex, endIndex - startIndex);          // Set a string retVal to the data stored in between <...>

        if (logging)                                                                // More Debug Messages
        {
            Debug.Log("Parser: Found value of field " + field + " to be " + retVal);
        }

        return retVal;                                                              // Return retVal to calling function
    }
}
