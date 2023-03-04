using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool active = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<CharacterController2D>().currentCheckpoint != this.gameObject)
        {
            other.gameObject.GetComponent<CharacterController2D>().currentCheckpoint.GetComponent<Checkpoint>().deactivate();
            other.gameObject.GetComponent<CharacterController2D>().currentCheckpoint = gameObject;
            activate();
            Debug.Log("Changed checkpoint");
        }
    }

    public void activate()
    {
        active = true;
    }

    public void deactivate()
    {
        active = false;
    }
}
