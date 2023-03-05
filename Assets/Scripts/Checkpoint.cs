using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public Animator animator;

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.CompareTag ("Player") && other.gameObject.GetComponent<CharacterController2D> ().currentCheckpoint != this.gameObject) {
            other.gameObject.GetComponent<CharacterController2D> ().currentCheckpoint.GetComponent<Checkpoint> ().deactivate ();
            other.gameObject.GetComponent<CharacterController2D> ().currentCheckpoint = gameObject;
            activate ();
            Debug.Log ("Changed checkpoint");
        }
    }

    public void activate () {
        animator.SetTrigger ("Activate");
    }

    public void deactivate () {
        animator.SetTrigger ("Deactivate");
    }
}