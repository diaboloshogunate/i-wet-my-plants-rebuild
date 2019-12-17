using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Plant : MonoBehaviour
{
    private Animator animator;
    public UnityEvent levelUpEvent;
    public UnityEvent levelDownEvent;
    [SerializeField] private int level = 1;
    [SerializeField] private int maxLevel = 1;
    [SerializeField] private float consumption = 5f;

    private void Start()
    {
        this.animator = GetComponent<Animator>();
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

    private void Water(Ball player)
    {
        player.UseWater(this.consumption);
        this.level = Mathf.Clamp(this.level + 1, 0, this.maxLevel);
        this.animator.SetInteger("Level", this.level);
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
