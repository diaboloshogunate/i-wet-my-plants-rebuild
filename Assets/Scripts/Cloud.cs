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
    [SerializeField] private Transform puddleSpawnPosition;
    [SerializeField] private Vector2 spawnXBounds;
    [SerializeField] private Vector2 spawnYBounds;
    [SerializeField] private float minTargetDistance = 2f;
    [SerializeField] private Vector2 respawnTime = Vector2.zero;
    [SerializeField] private Vector2 speed = Vector2.zero;
    private bool isSpawned = true;
    private bool isMoving = true;
    private float timeToSpawn = 5f;
    Vector2 spawnPosition = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;

    private void Awake()
    {
        this.anim = GetComponent<Animator>();
        this.Spawn();
    }

    private void Update()
    {
        if (this.isSpawned)
        {
            if (isMoving)
            {
                this.MoveTo();
            }

            return;
        }
        
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
        this.isSpawned = true;
        this.isMoving = true;
        this.TargetPosition();
        this.anim.ResetTrigger("Die");
        this.anim.SetTrigger("Spawn");
    }

    private void TargetPosition()
    {
        do
        {
            this.targetPosition.x = Random.Range(spawnXBounds.x, spawnXBounds.y);
            this.targetPosition.y = Random.Range(spawnYBounds.x, spawnYBounds.y);
        } while (Vector2.Distance(this.spawnPosition, this.targetPosition) > this.minTargetDistance);
    }

    private void MoveTo()
    {
        float step = Random.Range(this.speed.x, this.speed.y) * Time.deltaTime;
        this.transform.position = Vector2.MoveTowards(this.transform.position, this.targetPosition, step);
        if (Vector2.Distance(this.transform.position, this.targetPosition) < 0.01f)
        {
            this.Puddle();
        }
    }

    private void Puddle()
    {
        this.isMoving = false;
        GameObject puddle = Instantiate(this.puddle);
        puddle.transform.position = this.puddleSpawnPosition.position;
        StartCoroutine(this.Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(2);

        this.anim.ResetTrigger("Spawn");
        this.anim.SetTrigger("Die");
        this.timeToSpawn = Random.Range(this.respawnTime.x, this.respawnTime.y);
        this.isSpawned = false;
    }
}
