using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    
    private bool isLocked = true;
    [SerializeField] private Transform spawn;
    private float speed;
    [SerializeField] private float startSpeed = 1f;
    [SerializeField] private float finalSpeed = 10f;
    [SerializeField] private float acceleration = 0.1f;

    private void Start()
    {
        this.rigidbody = this.GetComponent<Rigidbody2D>();
        this.Fire();
    }

    private void Update()
    {
        if (this.isLocked)
        {
            this.MoveToSpawn();
        }
    }

    public bool IsLocked()
    {
        return this.isLocked;
    }

    public void Fire()
    {
        if (!this.isLocked)
        {
            return;
        }
        
        this.isLocked = false;
        this.speed = this.startSpeed;
        this.rigidbody.velocity = new Vector2(0,this.speed);
    }

    public void Tilt()
    {
        //@TODO
    }

    public void Respawn()
    {
        this.isLocked = true;
        this.MoveToSpawn();
    }

    private void IncreaseSpeed()
    {
        this.speed = Mathf.Clamp(this.acceleration + this.acceleration, this.startSpeed, this.finalSpeed);
    }

    private void MoveToSpawn()
    {
        this.transform.position = this.spawn.position;
    }
}
