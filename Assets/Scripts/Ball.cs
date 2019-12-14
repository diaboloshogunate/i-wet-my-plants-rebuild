using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    
    private bool isLocked = true;
    [SerializeField] private Transform spawn;
    [SerializeField] private Transform pivot;
    [SerializeField] private BoxCollider2D paddleCollider;
    private float speed;
    [SerializeField] private float startSpeed = 1f;
    [SerializeField] private float finalSpeed = 10f;
    [SerializeField] private float acceleration = 0.1f;

    private void Start()
    {
        this.rigidbody = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (this.isLocked)
        {
            this.MoveToSpawn();
        }
    }

    private void FixedUpdate()
    {
        this.rigidbody.velocity = Vector3.ClampMagnitude(this.rigidbody.velocity, this.speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //@TODO track contact points
        if (other.gameObject.CompareTag("Paddle"))
        {
            this.PaddleBounce(other);
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

    private void PaddleBounce(Collision2D other)
    {
        if (other.contacts[0].point.y < other.collider.bounds.max.y)
        {
            return;
        }
        
        this.IncreaseSpeed();
        Vector2 direction = this.transform.position - this.pivot.position;
        this.rigidbody.velocity = direction.normalized * this.speed;
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
