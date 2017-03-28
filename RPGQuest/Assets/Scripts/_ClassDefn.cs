// Put enums and non-component class definitions here.
// Brandon

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

//Interfaces

//Enums
public enum GAMESTATE
{
    /// <summary>
    /// Holds enum values for the state manager.
    /// </summary>
    Explore = 0,
    Battle
};
public enum NAVMODE
{
    //Holds enum values for navigation modes used by the FieldMove4Dir script in determining basic movement for field objects. - B
    Player = 0,
    Fixed,
    Random,
    Freeze,
    Follow,
    PartyFollow
}
public class _ClassDefn : MonoBehaviour {

	//This is empty on purpose. Don't change it. -B

}
