using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour, iGameState {

    [Header("UI Variables")]
    public GameObject firstSelected;
    public StandaloneInputModule io;
    public List<Button> buttons;
    public Image gameLogo;


    private float fadeSpeed = 0.0125f;
    private bool isLoaded = false;
    public void register()
    {
        GameState.register(this.gameObject);
    }
    public void deregister()
    {
        GameState.deregister(this.gameObject);
    }
    public void changeGameState()
    {
        // Null
    }
    public void checkState(eGameState state)
    {
        if (!isLoaded)
        {
            if (state == eGameState.MainiMenu)
            {
                StartCoroutine(fadeIn());
            }
        }
    }
    IEnumerator fadeIn()
    {
        AudioManager.playMusic(eMusic.MainMenuTheme, true);                     // Queue up the main menu theme music
        AudioManager.fadeIn();                                                  // Start the couroutine to fade the music in
        this.GetComponent<Canvas>().enabled = true;                             // Enable the main menu canvas
        for (float f = 0; f < 2.0f; f += fadeSpeed)
        {
            gameLogo.GetComponent<CanvasRenderer>().SetAlpha(f);
            yield return new WaitForSeconds(fadeSpeed);
        }
        for(float f = 0; f < 2.0f; f += fadeSpeed)
        {
            foreach (Button but in buttons)
            {
                but.GetComponent<CanvasRenderer>().SetAlpha(f);
            }
            yield return new WaitForSeconds(fadeSpeed);
        }

        // The loop terminates with an alpha value a little below 2.0. so here we go ahead an set it to 2.0
        // to ensure it is completely visible!
        gameLogo.GetComponent<CanvasRenderer>().SetAlpha(2.0f);
        foreach(Button but in buttons)
        {
            but.GetComponent<CanvasRenderer>().SetAlpha(2.0f);
        }
        
        firstSelected.GetComponent<Button>().Select();                          // Select the first button, New Game, for keyboard navigation
        isLoaded = true;                                                        // Let the menu controller know it is loaded
        yield return null;                                                      // End this Coroutine
    }
    IEnumerator fadeOut(eGameState which)
    {
        EventSystem.current.SetSelectedGameObject(null);                        // Disable the standalone input manager
        AudioManager.fadeOut();                                                 // Fade out the main menu music
        for(float f = 2.0f; f > 0.0f; f -= fadeSpeed)                           // Fade out the buttons first
        {
            foreach(Button but in buttons)                                      // Iterate through the buttons list so each one gets hit
            {
                but.GetComponent<CanvasRenderer>().SetAlpha(f);                 // Set the alpha
            }
            yield return new WaitForSeconds(fadeSpeed);                         // Wait for some amount of time
        }
        for(float f = 2.0f; f > 0.0f; f -= fadeSpeed)                           // Fade out the game logo
        {
            gameLogo.GetComponent<CanvasRenderer>().SetAlpha(f);                // Set the alpha
            yield return new WaitForSeconds(fadeSpeed);                         // Wait for some amount of time
        }

        gameLogo.GetComponent<CanvasRenderer>().SetAlpha(0.0f);                 // Set the gameLogo to exactly 0 alpha
        foreach(Button but in buttons)
        {
            but.GetComponent<CanvasRenderer>().SetAlpha(0.0f);                  // Set the buttons to exactly 0 alpha
        }
        isLoaded = false;                                                       // Tell the main menu controller it is disabled
        this.GetComponent<Canvas>().enabled = false;                            // Disable the main menu UI
        GameState.changeState(which);                                           // Send along what the new game state is
        yield return null;                                                      // Return
       
    }

    void Awake()
    {
        register();
        this.gameObject.GetComponent<Canvas>().enabled = true;
        foreach(Button but in buttons)
        {
            but.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        }
        gameLogo.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
    }
    public void newGame()
    {
        AudioManager.playSFX(eSFX.AcceptSelection);
        if (SettingInfo.logging)
        {
            Debug.Log("New Game Selected");
        }
    }
    public void loadGame()
    {
        AudioManager.playSFX(eSFX.AcceptSelection);
        if (SettingInfo.logging)
        {
            Debug.Log("Load Game Selected");
        }
        StartCoroutine(fadeOut(eGameState.LoadGameMenu));
    }
    public void settings()
    {
        AudioManager.playSFX(eSFX.AcceptSelection);
        if (SettingInfo.logging)
        {
            Debug.Log("Settings Selected");
        }
    }
    public void credits()
    {
        AudioManager.playSFX(eSFX.AcceptSelection);
        if (SettingInfo.logging)
        {
            Debug.Log("Credits Selected");
        }
    }
    public void exit()
    {
        AudioManager.playSFX(eSFX.AcceptSelection);
        if (SettingInfo.logging)
        {
            Debug.Log("Exit Selected");
        }
    }

    public void navigationEvent()
    {
        AudioManager.playSFX(eSFX.MoveSelection);
    }
}
