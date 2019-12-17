﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private Ball ball;
    [SerializeField] private Image waterMeter;
    [SerializeField] private Image progressMeter;
    [SerializeField] private Image flower;
    private List<Plant> plants;
    private float level;
    private float maxLevel;

    private void Start()
    {
        this.ball.killedEvent.AddListener(OnKilled);
        this.ball.waterEvent.AddListener(OnWaterConsumed);
        this.plants = FindObjectsOfType<Plant>().ToList();
        this.plants.ForEach(o =>
        {
            o.levelUpEvent.AddListener(OnLevelUp);
            this.maxLevel += o.GetMaxLevel();
            this.level += o.GetLevel();
        });
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

    private void OnLevelUp()
    {
        this.level++;
        this.OnLevelChange();
    }

    private void OnLevelDown()
    {
        this.level--;
        this.OnLevelChange();
    }

    private void OnLevelChange()
    {
        this.level = Mathf.Clamp(this.level, 0, this.maxLevel);
        this.progressMeter.fillAmount = this.level / this.maxLevel;
        if (this.progressMeter.fillAmount == 1f)
        {
            this.flower.enabled = true;
        }
        Debug.Log("Level up " + this.progressMeter.fillAmount);
    }

    private void GameOver()
    {
        this.sceneController.SetScene("GameOver");
        this.sceneController.LoadNextScene();
    }
}
