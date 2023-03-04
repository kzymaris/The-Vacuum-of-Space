using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public GameObject player;
    
    private void OnCollisionEnter2D (Collision2D other) {

        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<CharacterController2D>().reset();
            Debug.Log("You suck");
        }
    
    }
}
