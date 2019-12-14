using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Rigidbody2D))]
public class PaddleController : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Camera camera;
    private Player player;
    private bool isGamepad = false;
    private Ball ball;
    private Vector2 yzPosition = Vector2.zero; 
    private Vector2 direction = Vector2.zero;
    [SerializeField] private float speed;
    [SerializeField] private float minBounds;
    [SerializeField] private float maxBounds;
    
    void Start()
    {
        this.camera = Camera.main;
        this.player = ReInput.players.GetPlayer(0);
        this.ball = FindObjectOfType<Ball>();
        this.rigidbody = this.GetComponent<Rigidbody2D>();
        this.yzPosition.x = this.transform.position.y;
        this.yzPosition.y = this.transform.position.z;
        this.CheckGamepad();
        ReInput.ControllerConnectedEvent += CheckGamepad;
        ReInput.ControllerDisconnectedEvent += CheckGamepad;
    }

    private void OnDestroy()
    {
        ReInput.ControllerConnectedEvent -= CheckGamepad;
        ReInput.ControllerDisconnectedEvent -= CheckGamepad;
    }

    void Update()
    {
        if (this.player.GetButton("Fire") && this.ball.IsLocked())
        {
            this.ball.Fire();
        }

        if (this.player.GetButton("Splash"))
        {
            Debug.Log("Splash");
        }

        if (this.isGamepad)
        {
            this.direction.x = this.player.GetAxis("Paddle");
            this.direction.Normalize();
        }
        else
        {
            this.direction = Vector2.zero;
            Vector3 position = camera.ScreenToWorldPoint(ReInput.controllers.Mouse.screenPosition);
            position.x = Mathf.Clamp(position.x, this.minBounds, this.maxBounds);
            position.y = this.yzPosition.x;
            position.z = this.yzPosition.y;
            this.transform.position = position;
        }
    }

    private void FixedUpdate()
    {
        this.rigidbody.velocity = this.direction * this.speed;
    }

    private void CheckGamepad()
    {
        this.isGamepad = this.player.controllers.joystickCount > 0;
    }
    
    private void CheckGamepad(ControllerStatusChangedEventArgs args) {
        this.CheckGamepad();
    }
}
