using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WaterSource : MonoBehaviour
{
    private bool hasBeenUsed = false;
    private Animator animator;
    [SerializeField] private float amount = 10f;
    
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!this.hasBeenUsed && other.gameObject.CompareTag("Player"))
        {
            this.hasBeenUsed = true;
            other.gameObject.GetComponent<Ball>().UseWater(-1*this.amount);
            this.animator.SetTrigger("Dry");
        }
    }
}
