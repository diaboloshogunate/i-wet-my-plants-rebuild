using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Rewired;
using Sirenix.OdinInspector;
using UnityEditor;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private Transition transition;
    [SerializeField] private VideoPlayer loadAfterVideo;
    [SerializeField] private bool anyKey;
    [SerializeField] private string[] buttons;
    [SerializeField] private bool quitOnEscape = false;
    [EnableIf("@this.quitOnEscape == false")] [SerializeField] private string quitScene;
    [SerializeField] private string[] quitOnButtons;
    private bool hasStarted = false;
    private Player player;
    
    private void Awake()
    {
        this.player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        if (this.quitOnEscape && Input.GetKey(KeyCode.Escape))
        {
            this.Quit();
            return;
        }

        foreach (string button in this.quitOnButtons)
        {
            if (this.player.GetButton(button))
            {
                this.Quit();
                return;
            }
        }

        if (this.hasStarted) return;
        
        if (loadAfterVideo && loadAfterVideo.isPrepared && !loadAfterVideo.isPlaying)
        {
            this.LoadNextScene();
        }

        if (this.anyKey && (Input.anyKey || this.player.GetAnyButton()))
        {
            this.LoadNextScene();
        }

        foreach (string button in this.buttons)
        {
            if (this.player.GetButton(button))
            {
                this.LoadNextScene();
            }
        }
    }

    public void LoadNextScene()
    {
        this.hasStarted = true;
        if (this.transition)
        {
            StartCoroutine("FadeIn");
        }
        else
        {
            SceneManager.LoadScene(this.nextScene);
        }
    }

    IEnumerator FadeIn()
    {
        while (!this.transition.IsFadeInReady())
        {
            yield return null; 
        }
        
        this.transition.FadeIn();
        
        while (!this.transition.IsFadeOutReady())
        {
            yield return null; 
        }
        
        SceneManager.LoadScene(this.nextScene);
    }

    public void SetScene(string scene)
    {
        this.nextScene = scene;
    }

    public string GetScene()
    {
        return this.nextScene;
    }

    public void Quit()
    {
        if (!this.quitOnEscape)
        {
            this.nextScene = this.quitScene;
            this.LoadNextScene();
            return;
        }
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
