using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Magnets : MonoBehaviour
{
    public float MagnetForce = 10f;
    public float MagnetDistance = 3.5f;
    public Rigidbody2D Player;
    public Rigidbody2D AllTheMagnets;
    bool Magnet = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown ("Magnet")){
            if (Magnet){
                Magnet = false;
            }
            else{
                Magnet = true;
            }
        }

        if (Magnet){
            if ((Player.position - AllTheMagnets.position).magnitude < MagnetDistance){
                Player.AddForce((Player.position - AllTheMagnets.position).normalized * (-MagnetForce / (AllTheMagnets.position - Player.position).magnitude));
            }
        }
    }
}
