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
        if(state == eGameState.MainiMenu)
        {
            StartCoroutine(fadeIn());
        }
        else
        {
            
            this.gameObject.GetComponent<Canvas>().enabled = false;
        }
    }
    IEnumerator fadeIn()
    {
        AudioManager.playMusic(eMusic.MainMenuTheme, true);
        AudioManager.fadeIn();
        
        this.GetComponent<Canvas>().enabled = true;
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
        firstSelected.GetComponent<Button>().Select();
        
        yield return null;
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
        if (SettingInfo.logging)
        {
            Debug.Log("Load Game Selected");
        }
    }
    public void settings()
    {
        if (SettingInfo.logging)
        {
            Debug.Log("Settings Selected");
        }
    }
    public void credits()
    {
        if (SettingInfo.logging)
        {
            Debug.Log("Credits Selected");
        }
    }
    public void exit()
    {
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
