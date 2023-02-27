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

	bool jump = false;
	bool unjump = true;
	bool rejump = false;
	bool slam = false;
	bool dash = false;
	float dashTimer = 0;
	float slamTimer = 0;
	float jumpTimer = 0;
	public float dashCooldown = .8f;
	public float jumpCooldown = .8f;

	bool magnetOn = false;

	public void Start () {
		controller.Landed += onLanded;
	}

	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw ("Horizontal") * runSpeed;

		verticalMove = Input.GetAxisRaw ("Vertical") * runSpeed;

		if (Input.GetAxisRaw ("Vertical") > 0) {
			jump = true;
			unjump = false;
		} else if (Input.GetAxisRaw ("Vertical") < 0) {
			slam = true;
		} else if (Input.GetAxisRaw ("Vertical") == 0) {
			unjump = true;
			rejump = false;
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
		if (jump && !(magnetOn && controller.m_OnWall)) {
			if (rejump) {
				jump = false;
			}
			if (Time.time < jumpTimer) {
				jump = false;
			} else {
				rejump = true;
				jumpTimer = Time.time + jumpCooldown;
				JumpLight.color = Color.red;
			}
		}

		if (slam && !(magnetOn && controller.m_OnWall)) {
			if (Time.time < slamTimer) {
				slam = false;
			} else {
				slamTimer = Time.time + dashCooldown;
				SlamLight.color = Color.red;
			}
		}

		if (dash) {
			if (Time.time < dashTimer) {
				dash = false;
			} else {
				dashTimer = Time.time + dashCooldown;
				DashLight.color = Color.red;
			}
		}

		controller.Move (horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, dash, jump, unjump, slam, magnetOn);
		jump = false;
		dash = false;
		slam = false;

		if (Time.time > dashTimer) {
			DashLight.color = Color.green;
		}
		if (Time.time > jumpTimer) {
			JumpLight.color = Color.green;
		}
		if (Time.time > slamTimer) {
			SlamLight.color = Color.green;
		}
	}

	public void onLanded () {
		jumpTimer = 0;
	}
}