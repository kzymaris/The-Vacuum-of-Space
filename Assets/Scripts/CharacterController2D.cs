using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour {
    [SerializeField] private float m_JumpForce = 400f; // Amount of force added when the player jumps.
    [SerializeField] private float m_SlamForce = 400f;
    [SerializeField] private float m_DashForce = 700f;
    [Range (0, .3f)][SerializeField] private float m_MovementSmoothing = .05f; // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false; // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround; // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck; // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck; // A position marking where to check for ceilings
    [SerializeField] private Transform m_WallCheck;
    [SerializeField] private PhysicsMaterial2D slippy;
    [SerializeField] private PhysicsMaterial2D sticky;
    [SerializeField] private float m_Gravity = 3f;
    [SerializeField] public GameObject currentCheckpoint;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded; // Whether or not the player is grounded.
    public bool m_OnWall; // Whether or not the player is grounded.
    const float k_WallRadius = .1f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    public bool m_FacingRight = true; // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;
    private bool dashing = false;
    [SerializeField] private float dashTime = 0.1f;
    private float dashTimer;
    private bool jumping = false;
    [SerializeField] private float jumpTime = 0.5f;
    private float jumpTimer;

    public delegate void LandHandler ();
    public event LandHandler Landed;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake () {
        m_Rigidbody2D = GetComponent<Rigidbody2D> ();
    }

    private void FixedUpdate () {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        m_OnWall = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll (m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                m_Grounded = true;
                if (!wasGrounded) {
                    Landed.Invoke ();
                }
            }
        }

        colliders = Physics2D.OverlapCircleAll (m_WallCheck.position, k_WallRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                m_OnWall = true;
            }
        }

    }

    public void Move (float move, float verticalMove, bool dash, bool jump, bool unjump, bool slam, bool magnetOn) {
        if (jumping) {
            m_Rigidbody2D.velocity = new Vector2 (m_Rigidbody2D.velocity.x, m_JumpForce);
            if (unjump || Time.time > jumpTimer) {
                jumping = false;
            }
        }
        if (!dashing) {
            gameObject.GetComponent<Rigidbody2D> ().gravityScale = m_Gravity;
            if (move == 0f && !dash && m_Grounded) {
                gameObject.GetComponent<Rigidbody2D> ().sharedMaterial = sticky;
            } else {
                gameObject.GetComponent<Rigidbody2D> ().sharedMaterial = slippy;
            }

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl || m_OnWall) {
                if ((m_OnWall && magnetOn) && move == 0 && verticalMove == 0) {
                    m_Rigidbody2D.velocity = Vector2.zero;
                    gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0f;
                }
                if ((m_OnWall && magnetOn) && (move > 0 && m_FacingRight || move < 0 && !m_FacingRight || verticalMove != 0)) {
                    float wallSpeed;
                    if (verticalMove != 0) {
                        wallSpeed = verticalMove;
                    } else {
                        wallSpeed = Math.Abs (move);
                    }

                    Vector3 targetVelocity = new Vector2 (0, wallSpeed * 10f);

                    m_Rigidbody2D.velocity = Vector3.SmoothDamp (m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

                } else {

                    // Move the character by finding the target velocity
                    Vector3 targetVelocity = new Vector2 (move * 10f, m_Rigidbody2D.velocity.y);
                    // And then smoothing it out and applying it to the character
                    m_Rigidbody2D.velocity = Vector3.SmoothDamp (m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

                    // If the input is moving the player right and the player is facing left...
                    if (move > 0 && !m_FacingRight) {
                        // ... flip the player.
                        Flip ();
                    }
                    // Otherwise if the input is moving the player left and the player is facing right...
                    else if (move < 0 && m_FacingRight) {
                        // ... flip the player.
                        Flip ();
                    }
                }

            }
            // If the player should jump...
            if (jump && !(m_OnWall && magnetOn)) {
                // Add a vertical force to the player.
                m_Grounded = false;
                jumping = true;
                jumpTimer = Time.time + jumpTime;
            }

            if (!m_Grounded && slam && !(m_OnWall && magnetOn)) {
                m_Rigidbody2D.AddForce (new Vector2 (0f, m_SlamForce * -1));
            }

            if (dash) {
                dashing = true;
                dashTimer = Time.time + dashTime;
            }
        } else {
            m_Rigidbody2D.velocity = new Vector2 (m_FacingRight ? m_DashForce : -1 * m_DashForce, m_Rigidbody2D.velocity.y);
            if (Time.time > dashTimer) {
                dashing = false;
                m_Rigidbody2D.velocity = Vector2.zero;
            }
        }
    }

    private void Flip () {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void reset () {
        transform.position = currentCheckpoint.transform.position;
        m_Rigidbody2D.velocity = Vector2.zero;
        currentCheckpoint.GetComponent<Checkpoint> ().animator.SetTrigger ("Respawn");
    }
}