using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Magnets : MonoBehaviour {
    public float MagnetForce = 10f;
    public float MagnetDistance = 3.5f;
    public Rigidbody2D Player;
    public Rigidbody2D MagnetBody;
    bool Magnet = false;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown ("Magnet")) {
            Magnet = !Magnet;
        }

        if (Magnet) {
            if ((Player.position - MagnetBody.position).magnitude < MagnetDistance) {
                Player.AddForce ((Player.position - MagnetBody.position).normalized * (-MagnetForce / (MagnetBody.position - Player.position).magnitude));
            }
        }
    }
}