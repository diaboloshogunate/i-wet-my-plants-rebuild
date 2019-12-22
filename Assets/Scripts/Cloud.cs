using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class Cloud : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject puddle;
    [SerializeField] private Vector2 spawnXBounds;
    [SerializeField] private Vector2 spawnYBounds;
    [SerializeField] private float minTargetDistance = 2f;
    [SerializeField] private Vector2 respawnTime = Vector2.zero;
    private bool isSpawned = true;
    private float timeToSpawn = 5f;
    Vector2 spawnPosition = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;

    private void Awake()
    {
        this.anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(this.isSpawned) return;
        
        this.timeToSpawn -= Time.deltaTime;
        if (this.timeToSpawn <= 0f)
        {
            this.Spawn();
        }
    }

    public void Spawn()
    {
        this.spawnPosition.x = Random.Range(spawnXBounds.x, spawnXBounds.y);
        this.spawnPosition.y = Random.Range(spawnYBounds.x, spawnYBounds.y);
        this.transform.position = spawnPosition;
        this.anim.SetTrigger("Spawn");
        this.isSpawned = true;

        do
        {
            this.targetPosition.x = Random.Range(spawnXBounds.x, spawnXBounds.y);
            this.targetPosition.y = Random.Range(spawnYBounds.x, spawnYBounds.y);
        } while (Vector2.Distance(this.spawnPosition, this.targetPosition) > this.minTargetDistance);
    }

    private void MoveTo()
    {
        //@TODO move to target based with coroutine
    }

    private void Puddle()
    {
        //@TODO spawn puddle then die
    }

    private void Die()
    {
        this.anim.SetTrigger("Die");
        this.timeToSpawn = Random.Range(this.respawnTime.x, this.respawnTime.y);
        this.isSpawned = false;
    }
}
