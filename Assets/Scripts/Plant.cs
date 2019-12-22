using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;

[RequireComponent(typeof(SkeletonAnimation))]
public class Plant : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    public UnityEvent levelUpEvent;
    public UnityEvent levelDownEvent;
    [SerializeField] private int level = 1;
    [SerializeField] private int maxLevel = 1;
    [SerializeField] private float consumption = 5f;

    private void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.SetAnimation(0, "Side_Tree_"+this.level, true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && this.level < this.maxLevel)
        {
            this.Water(other.gameObject.GetComponent<Ball>());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && this.level < this.maxLevel)
        {
            this.Water(other.gameObject.GetComponent<Ball>());
        }
    }

    public void Water(Ball player)
    {
        player.UseWater(this.consumption);
        this.level = Mathf.Clamp(this.level + 1, 0, this.maxLevel);
        skeletonAnimation.AnimationState.SetAnimation(0, "Side_Tree_"+this.level, true);
        this.levelUpEvent.Invoke();
    }

    public int GetMaxLevel()
    {
        return this.maxLevel;
    }

    public int GetLevel()
    {
        return this.level;
    }
}
