using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public GameObject player;
    public GameObject startPoint;
    
    private void OnCollisionEnter2D (Collision2D other) {

        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.position = startPoint.transform.position;
            Debug.Log("You suck");
        }
    
    }
}
