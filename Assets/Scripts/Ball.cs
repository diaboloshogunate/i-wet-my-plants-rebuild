using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private bool isLocked = true;
    public UnityEvent killedEvent;
    public UnityEvent waterEvent;
    [SerializeField] private Transform spawn;
    [SerializeField] private Transform pivot;
    [SerializeField] private BoxCollider2D paddleCollider;
    private float speed;
    [SerializeField] private float startSpeed = 1f;
    [SerializeField] private float finalSpeed = 10f;
    [SerializeField] private float acceleration = 0.1f;
    private Queue<Vector2> hits = new Queue<Vector2>();
    private int hitRepeatCount;
    [SerializeField] private float water;
    [SerializeField] private float waterCapacity = 100f;
    [SerializeField] private float hitUsage = 1f;
    [SerializeField] private float splashUsage = 10f;
    [SerializeField] private float splashRadius = 1f;//@TODO tie to an animation?
    [SerializeField] private float respawnPenalty = 10f;

    private void Start()
    {
        if (this.killedEvent == null)
        {
            this.killedEvent = new UnityEvent();
        }
        
        if (this.waterEvent == null)
        {
            this.waterEvent = new UnityEvent();
        }

        this.water = this.waterCapacity;
        this.waterEvent.Invoke();
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
        if (this.isLocked) { return; }
        this.CheckRepeting();
        
        if (other.gameObject.CompareTag("Paddle"))
        {
            this.PaddleBounce(other);
            this.UseWater(this.hitUsage);
            return;
        }
        
        if (other.gameObject.CompareTag("Plant"))
        {
            this.UseWater(this.hitUsage);
            //@TODO water plant
            return;
        }
        
        if (other.gameObject.CompareTag("Water"))
        {
            //@TODO absorb water
            return;
        }
        
        this.UseWater(this.hitUsage);
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

    public void CheckRepeting()
    {
        this.hits.Enqueue(this.transform.position);

        while (this.hits.Count > 12)
        {
            this.hits.Dequeue();
        }

        this.hitRepeatCount = 0;
        foreach (Vector2 hit in this.hits)
        {
            if (Vector2.Distance(this.transform.position, hit) < 0.1f)
            {
                this.hitRepeatCount++;
            }
        }
        
        if (this.hitRepeatCount >= 3)
        {
            //@TODO suggest tilt
        }
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
        this.UseWater(this.respawnPenalty);
        this.MoveToSpawn();
    }

    private void IncreaseSpeed()
    {
        this.speed = Mathf.Clamp(this.speed + this.acceleration, this.startSpeed, this.finalSpeed);
    }

    private void MoveToSpawn()
    {
        this.transform.position = this.spawn.position;
    }

    public void Kill()
    {
        this.killedEvent.Invoke();
    }

    public void UseWater(float amt)
    {
        this.water = Mathf.Clamp(this.water - amt,0, this.waterCapacity);
        this.waterEvent.Invoke();
    }

    public float GetWaterFill()
    {
        return this.water / this.waterCapacity;
    }
}
