using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool slam = false;
	bool dash = false;
	float dashTimerR = 0;
	float dashTimerL = 0;
	float slamTimer = 0;
	float jumpTimer = 0;
	public float dashCooldown = .8f;

	bool magnetOn = false;

	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw ("Horizontal") * runSpeed;

		if (Input.GetAxisRaw ("Vertical") > 0 && Time.time > jumpTimer) {
			jump = true;
			jumpTimer = Time.time + dashCooldown;

		} else if (Input.GetAxisRaw ("Vertical") < 0 && Time.time > slamTimer) {
			slam = true;
			slamTimer = Time.time + dashCooldown;
		}

		if (Input.GetButtonDown ("Dash")) {
			if (Time.time > dashTimerR && controller.m_FacingRight) {
				dash = true;
				dashTimerR = Time.time + dashCooldown;
			}
			if (Time.time > dashTimerL && !controller.m_FacingRight) {
				dash = true;
				dashTimerL = Time.time + dashCooldown;
			}
		}

		if (Input.GetButtonDown ("Magnet")) {
			magnetOn = !magnetOn;
		}
	}

	void FixedUpdate () {
		// Move our character
		controller.Move (horizontalMove * Time.fixedDeltaTime, dash, jump, slam, magnetOn);
		jump = false;
		dash = false;
		slam = false;
	}
}