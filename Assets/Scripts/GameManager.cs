using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private Ball ball;
    [SerializeField] private Image waterMeter;
    [SerializeField] private Image progressMeter;

    private void Start()
    {
        this.ball.killedEvent.AddListener(OnKilled);
        this.ball.waterEvent.AddListener(OnWaterConsumed);
    }

    private void OnDestroy()
    {
        this.ball.killedEvent.RemoveListener(OnKilled);
        this.ball.waterEvent.RemoveListener(OnWaterConsumed);
    }

    private void OnKilled()
    {
        if (this.ball.GetWaterFill() > 0f)
        {
            this.ball.Respawn();
            return;
        }
        
        this.GameOver();
    }

    private void OnWaterConsumed()
    {
        this.waterMeter.fillAmount = this.ball.GetWaterFill();
    }

    private void GameOver()
    {
        this.sceneController.SetScene("GameOver");
        this.sceneController.LoadNextScene();
    }
}
