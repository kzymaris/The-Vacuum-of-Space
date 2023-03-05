using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public CharacterController2D controller;
    public SpriteRenderer DashLight;
    public SpriteRenderer JumpLight;
    public SpriteRenderer SlamLight;
    public SpriteRenderer MagLight;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    float verticalMove = 0f;

    bool jumping = false;
    bool startJump = false;
    bool slam = false;
    bool dash = false;
    float dashCooldownTimer = 0;
    float slamCooldownTimer = 0;
    float jumpCooldownTimer = 0;
    public float dashCooldown = .8f;
    public float jumpCooldown = .8f;
    [SerializeField] private float jumpBuffer = .3f;
    [SerializeField] private float jumpTime = 0.5f;
    private float jumpTimer;
    private float buffer;

    bool magnetOn = false;

    public void Start () {
        controller.Landed += onLanded;
    }

    // Update is called once per frame
    void Update () {

        horizontalMove = Input.GetAxisRaw ("Horizontal") * runSpeed;

        verticalMove = Input.GetAxisRaw ("Vertical") * runSpeed;

        if (Input.GetAxisRaw ("Vertical") < 0) {
            slam = true;
        }

        if (Input.GetButtonDown ("Jump")) {
            startJump = true;
            buffer = Time.time + jumpBuffer;
        }
        if (Input.GetButtonUp ("Jump")) {
            jumping = false;
        }

        if (Input.GetButtonDown ("Dash")) {
            dash = true;
        }

        if (Input.GetButtonDown ("Magnet")) {
            magnetOn = !magnetOn;
            if (magnetOn) {
                MagLight.color = Color.blue;
            } else {
                MagLight.color = Color.grey;
            }
        }
    }

    void FixedUpdate () {
        if (startJump || buffer > Time.time) {
            if (Time.time > jumpCooldownTimer) {
                jumpCooldownTimer = Time.time + jumpCooldown;
                jumpTimer = Time.time + jumpTime;
                jumping = true;
                JumpLight.color = Color.red;
            }
            startJump = false;
        }

        if (Time.time > jumpTimer) {
            jumping = false;
        }

        if (Time.time > jumpCooldownTimer) {
            JumpLight.color = Color.green;
        }

        if (slam && !(magnetOn && controller.m_OnWall)) {
            if (Time.time < slamCooldownTimer) {
                slam = false;
            } else {
                slamCooldownTimer = Time.time + dashCooldown;
                SlamLight.color = Color.red;
            }
        }

        if (dash) {
            if (Time.time < dashCooldownTimer) {
                dash = false;
            } else {
                dashCooldownTimer = Time.time + dashCooldown;
                DashLight.color = Color.red;
            }
        }

        controller.Move (horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, dash, jumping, slam, magnetOn);
        dash = false;
        slam = false;

        if (Time.time > dashCooldownTimer) {
            DashLight.color = Color.green;
        }
        if (Time.time > jumpCooldownTimer) {
            JumpLight.color = Color.green;
        }
        if (Time.time > slamCooldownTimer) {
            SlamLight.color = Color.green;
        }
    }

    public void onLanded () {
        jumpCooldownTimer = 0;
        JumpLight.color = Color.green;
    }
}