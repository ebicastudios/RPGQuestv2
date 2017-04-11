using UnityEngine;
using System.Collections;

public enum D_PARSER
{
    STRING = 0,
    INT
}

public class D_Parser : MonoBehaviour {

    [Header("Debug Flags")]
    public bool logging = false;
    public bool enabled = false;
    public D_PARSER parseType;

    [Header("Strings")]
    public string sourceString;
    public string fieldString;


    void Update()
    {
        if (enabled)
        {
            if (logging) { Parser.setLogging(true); }
            else { Parser.setLogging(false); }

            if (Input.GetKeyDown("`"))
            {
                if (parseType == D_PARSER.STRING)
                {
                    string testString = Parser.getFieldString(sourceString, fieldString);
                }
                else if (parseType == D_PARSER.INT)
                {
                    int testInt = Parser.getFieldInt(sourceString, fieldString);
                }
            }
        }
    }

}
