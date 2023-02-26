using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public SpriteRenderer RightLight;
	public SpriteRenderer LeftLight;
	public SpriteRenderer JumpLight;
	public SpriteRenderer SlamLight;
	public SpriteRenderer MagLight;

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
	public float jumpCooldown = .8f;

	bool magnetOn = false;

	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw ("Horizontal") * runSpeed;

		if (Input.GetAxisRaw ("Vertical") > 0) {
			jump = true;
		} else if (Input.GetAxisRaw ("Vertical") < 0) {
			slam = true;
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
		// Move our character
		if (jump) {
			if (Time.time < jumpTimer) {
				jump = false;
			} else {
				jumpTimer = Time.time + jumpCooldown;
				JumpLight.color = Color.red;
			}
		}

		if (slam) {
			if (Time.time < slamTimer) {
				slam = false;
			} else {
				slamTimer = Time.time + dashCooldown;
				SlamLight.color = Color.red;
			}
		}

		if (dash && controller.m_FacingRight) {
			if (Time.time < dashTimerR) {
				dash = false;
			} else {
				dashTimerR = Time.time + dashCooldown;
				RightLight.color = Color.red;
			}
		}
		if (dash && !controller.m_FacingRight) {
			if (Time.time < dashTimerL) {
				dash = false;
			} else {
				dashTimerL = Time.time + dashCooldown;
				LeftLight.color = Color.red;
			}
		}

		controller.Move (horizontalMove * Time.fixedDeltaTime, dash, jump, slam, magnetOn);
		jump = false;
		dash = false;
		slam = false;

		if (Time.time > dashTimerR) {
			RightLight.color = Color.green;
		}
		if (Time.time > dashTimerL) {
			LeftLight.color = Color.green;
		}
		if (Time.time > jumpTimer) {
			JumpLight.color = Color.green;
		}
		if (Time.time > slamTimer) {
			SlamLight.color = Color.green;
		}
	}
}