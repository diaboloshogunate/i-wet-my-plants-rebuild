using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class Transition : MonoBehaviour
{
    private Animator anim;
    private bool isFadeInReady = false;
    private bool isFadeOutReady = true;

    private void OnEnable()
    {
        this.anim = GetComponent<Animator>();
        SceneManager.sceneLoaded += this.OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= this.OnSceneLoaded;
    }

    private void SetFadeInReady()
    {
        this.isFadeInReady = true;
    }

    private void SetFadeOutReady()
    {
        this.isFadeOutReady = true;
    }

    public bool IsFadeInReady()
    {
        return this.isFadeInReady;
    }

    public bool IsFadeOutReady()
    {
        return this.isFadeOutReady;
    }

    public void FadeIn()
    {
        this.anim.ResetTrigger("FadeOut");
        this.anim.SetTrigger("FadeIn");
        this.isFadeOutReady = false;
    }

    public void FadeOut()
    {
        this.anim.ResetTrigger("FadeIn");
        this.anim.SetTrigger("FadeOut");
        this.isFadeInReady = false;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.FadeOut();
    }
}
