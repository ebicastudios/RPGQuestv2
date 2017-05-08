using UnityEngine;
using System.Collections;

public interface iSaveable
{
    string save();                                         // Method for handling saving. Returns a formatted string to the calling method
    void load(string what);                                // Method for handling loading. Loads data specified by "what"
}                                   // Implemented by all scripts that need to construct save/load data
public interface iGameState
{
    void changeGameState();                                     // Method for changing the game state
    void register();                                            // Method for registering as an observer
    void deregister();                                          // Method for deregistring as an observer

    void checkState(eGameState which);                          // Method for handling a game state change
}                                  // Implemented by all scripts that need to handle game state changes
