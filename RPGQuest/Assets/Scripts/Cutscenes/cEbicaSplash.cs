using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cEbicaSplash : MonoBehaviour, iGameState {

    #region Variables
    public bool isEnabled = false;
    public bool skip = false;
    public GameObject splashObj1;
    public GameObject splashObj2;
    public float fadeSpeed = .05f;
    #endregion

    #region Observer Pattern Behavior
    public void register()
    {
        GameState.register(this.gameObject);
    }                         // Register this object as an observer of the GameState
    public void deregister()
    {
        GameState.deregister(this.gameObject);
    }                       // Deregister this object as an observer of the GameState
    public void changeGameState()
    {
        //Not omplemented yet
    }                  // Signal the GameState to change
    public void checkState(eGameState which)
    {
        if(which == eGameState.SplashScreen)
        {
            isEnabled = true;
        }
        else
        {
            isEnabled = false;
        }

        if (isEnabled)
        {
            if (skip)
            {
                GameState.changeState(eGameState.MainiMenu);
                deregister();
                Destroy(this);
                return;
            }
            StartCoroutine(startCutscene());
        }
    }       // Detect a GameState change
    #endregion

    void Awake()
    {
        register();                        // Register this object with the GameState class
    }
    
    IEnumerator startCutscene()
    {
        Debug.Log("TEST1");
        // Get Camera Location
        Vector3 instantLocation = new Vector3(0, 0, 0);     // Holds the location of the camera
        instantLocation = Camera.main.transform.position;   // Get the position of the camera
        instantLocation.z = 0;                              // Make sure the screens are in front of the camera

        //First screen
        GameObject go = Instantiate(splashObj1, instantLocation, Quaternion.identity, GameObject.Find("CutsceneManager").GetComponent<Transform>()) as GameObject;
        for(float f = 0.0f; f < 2.0f; f += fadeSpeed)
        {
            Color newColor = go.GetComponent<SpriteRenderer>().color;
            newColor.a = f;
            go.GetComponent<SpriteRenderer>().color = newColor;
            yield return new WaitForSeconds(fadeSpeed);
        }

        yield return new WaitForSeconds(1.5f);

        for (float f = 2.0f; f > 0.0f; f -= fadeSpeed)
        {
            Color newColor = go.GetComponent<SpriteRenderer>().color;
            newColor.a = f;
            go.GetComponent<SpriteRenderer>().color = newColor;
            yield return new WaitForSeconds(fadeSpeed);
        }

        yield return new WaitForSeconds(1.5f);

        Destroy(go);

        // Second Screen
        go = Instantiate(splashObj2, instantLocation, Quaternion.identity, GameObject.Find("CutsceneManager").GetComponent<Transform>()) as GameObject;

        for (float f = 0.0f; f < 2.0f; f += fadeSpeed)
        {
            Color newColor = go.GetComponent<SpriteRenderer>().color;
            newColor.a = f;
            go.GetComponent<SpriteRenderer>().color = newColor;
            yield return new WaitForSeconds(fadeSpeed);
        }

        yield return new WaitForSeconds(1.5f);

        for (float f = 2.0f; f > 0.0f; f -= fadeSpeed)
        {
            Color newColor = go.GetComponent<SpriteRenderer>().color;
            newColor.a = f;
            go.GetComponent<SpriteRenderer>().color = newColor;
            yield return new WaitForSeconds(fadeSpeed);
        }

        yield return new WaitForSeconds(1.5f);

        Destroy(go);

        yield return null;

        deregister();
        GameState.changeState(eGameState.MainiMenu);
        Destroy(this);
    }
}
