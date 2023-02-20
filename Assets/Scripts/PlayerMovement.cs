using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;
	float dashTimer = 0;
	public float dashCooldown = 1f;
	
	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}

        if (Input.GetButtonDown("Dash") && Time.time > dashTimer)
		{
			dash = true;
			dashTimer = Time.time + dashCooldown;
		}
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, dash, jump);
		jump = false;
        dash = false;
	}
}