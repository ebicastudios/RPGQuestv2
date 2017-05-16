using UnityEngine;
using System.Collections;

public class AudioContainer : MonoBehaviour {

    [Header("Required Components")]
    public AudioSource musicSource;
    public AudioSource sfx1;
    public AudioSource sfx2;
    public AudioSource sfx3;
    public AudioSource sfx4;

    [Header("Music")]
    public AudioClip mainMenuTheme;

    [Header("SFX")]
    public AudioClip moveSelection;
    public AudioClip acceptSelection;

    private float fadeSpeed = 0.025f;
    void Awake()
    {
        AudioManager.register(this.gameObject);
    }

    #region Volume Fading
    public void fadeInMusic()
    {
        StartCoroutine(iFadeInMusic());
    }
    public void fadeOutMusic()
    {
        StartCoroutine(iFadeOutMusic());
    }
    private IEnumerator iFadeOutMusic()
    {
        for(float f = 1.0f; f > 0f; f -= fadeSpeed)
        {
            musicSource.volume = f;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }
    private IEnumerator iFadeInMusic()
    {
        for(float f = 0; f < 1.0f; f+=fadeSpeed)
        {
            musicSource.volume = f;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }
    #endregion
}
