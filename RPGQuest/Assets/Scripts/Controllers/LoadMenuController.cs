using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadMenuController : MonoBehaviour, iGameState {

    #region Variables
//     public bool isEnabled;
    public Text titleText;
    public List<Button> buttonFade = new List<Button>();
    public List<Image> savBackFade = new List<Image>();
    public List<Image> charBackFade = new List<Image>();
    public List<Text> buttonTextFade = new List<Text>();

    private PreviewFile prev1;
    private PreviewFile prev2;
    private PreviewFile prev3;

    private float fadeSpeed = 0.0125f;
    #endregion

    #region Observer Pattern and Initialization
    public void changeGameState() { }
    public void register() {
        GameState.register(this.gameObject);
    }
    public void deregister()
    {
        GameState.deregister(this.gameObject);
    }
    public void checkState(eGameState which) {
        if (which == eGameState.LoadGameMenu)
        {
            StartCoroutine(fadeIn());
        }
    }
    void Awake()
    {
        this.gameObject.GetComponent<Canvas>().enabled = false; 
        titleText.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        foreach(Button but in buttonFade)
        {
            but.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        }
        foreach(Image img in savBackFade)
        {
            img.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        }
        foreach(Image img in charBackFade)
        {
            img.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        }
        foreach(Text txt in buttonTextFade)
        {
            txt.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        }
        register();
    }
    #endregion

    #region Coroutines
    private IEnumerator fadeIn()
    {

        loadPreviews();
        for(float f = 0.0f; f < 2.0f; f += fadeSpeed)
        {
            titleText.GetComponent<CanvasRenderer>().SetAlpha(f);
            foreach(Button but in buttonFade)
            {
                but.GetComponent<CanvasRenderer>().SetAlpha(f);
            }
            foreach(Image img in savBackFade)
            {
                img.GetComponent<CanvasRenderer>().SetAlpha(f);
            }
            foreach(Image img in charBackFade)
            {
                img.GetComponent<CanvasRenderer>().SetAlpha(f);
            }
            foreach(Text txt in buttonTextFade)
            {
                txt.GetComponent<CanvasRenderer>().SetAlpha(f);
            }
            yield return new WaitForSeconds(fadeSpeed);
        }

        titleText.GetComponent<CanvasRenderer>().SetAlpha(2.0f);
        foreach (Button but in buttonFade)
        {
            but.GetComponent<CanvasRenderer>().SetAlpha(2.0f);
        }
        foreach(Image img in savBackFade)
        {
            img.GetComponent<CanvasRenderer>().SetAlpha(2.0f);
        }
        foreach(Image img in charBackFade)
        {
            img.GetComponent<CanvasRenderer>().SetAlpha(2.0f);
        }
        foreach(Text txt in buttonTextFade)
        {
            txt.GetComponent<CanvasRenderer>().SetAlpha(2.0f);
        }

        this.gameObject.GetComponent<Canvas>().enabled = true;
        yield return null;
    }
    private IEnumerator fadeOut()
    {
        this.gameObject.GetComponent<Canvas>().enabled = false;

        for (float f = 2.0f; f > 0.0f; f -= fadeSpeed)
        {
            titleText.GetComponent<CanvasRenderer>().SetAlpha(f);
            foreach (Button but in buttonFade)
            {
                but.GetComponent<CanvasRenderer>().SetAlpha(f);
            }
            foreach (Image img in savBackFade)
            {
                img.GetComponent<CanvasRenderer>().SetAlpha(f);
            }
            foreach (Image img in charBackFade)
            {
                img.GetComponent<CanvasRenderer>().SetAlpha(f);
            }
            foreach (Text txt in buttonTextFade)
            {
                txt.GetComponent<CanvasRenderer>().SetAlpha(f);
            }
            yield return new WaitForSeconds(fadeSpeed);
        }

        titleText.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        foreach (Button but in buttonFade)
        {
            but.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        }
        foreach (Image img in savBackFade)
        {
            img.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        }
        foreach (Image img in charBackFade)
        {
            img.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        }
        foreach (Text txt in buttonTextFade)
        {
            txt.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        }

        GameState.changeState(eGameState.MainiMenu);
        yield return null;
    }
    #endregion

    private void loadPreviews()
    {
        prev1 = new PreviewFile();
        prev2 = new PreviewFile();
        prev3 = new PreviewFile();

        prev1.load(FileManager.readFile(eFileList.MS1Preview));
        prev2.load(FileManager.readFile(eFileList.MS2Preview));
        prev3.load(FileManager.readFile(eFileList.MS3Preview));
    }                               // Load the preview files

    private void assignPreviews()                                // Assign the values of the preview files to the UI
    {

    }
    #region UI Methods
    public void onBackClick()
    {
        AudioManager.playSFX(eSFX.AcceptSelection);
        StartCoroutine(fadeOut());
    }
    #endregion
}                                   // Controller for the load menu
