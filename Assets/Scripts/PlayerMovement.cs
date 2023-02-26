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

		if (Input.GetAxisRaw ("Vertical") > 0 && Time.time > jumpTimer) {
			jump = true;
			jumpTimer = Time.time + jumpCooldown;
			JumpLight.color = Color.red;

		} else if (Input.GetAxisRaw ("Vertical") < 0 && Time.time > slamTimer) {
			slam = true;
			slamTimer = Time.time + dashCooldown;
			SlamLight.color = Color.red;
		}

		if (Input.GetButtonDown ("Dash")) {
			if (Time.time > dashTimerR && controller.m_FacingRight) {
				dash = true;
				dashTimerR = Time.time + dashCooldown;
				RightLight.color = Color.red;
			}
			if (Time.time > dashTimerL && !controller.m_FacingRight) {
				dash = true;
				dashTimerL = Time.time + dashCooldown;
				LeftLight.color = Color.red;
			}
		}

		if (Input.GetButtonDown ("Magnet")) {
			magnetOn = !magnetOn;
			if (magnetOn){
				MagLight.color = Color.blue;
			}
			else{
				MagLight.color = Color.grey;
			}

		}

		if (Time.time > dashTimerR){
			RightLight.color = Color.green;
		}
		if (Time.time > dashTimerL){
			LeftLight.color = Color.green;
		}
		if (Time.time > jumpTimer){
			JumpLight.color = Color.green;
		}
		if (Time.time > slamTimer){
			SlamLight.color = Color.green;
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