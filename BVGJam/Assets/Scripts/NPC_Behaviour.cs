using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Behaviour : MonoBehaviour {

    public int dialogIconOffsetHeight = 30;
    public Rigidbody2D rigidbody;

    //public float wander = 5f;

    public float speed = 4f;

    public string npcName = "FillerName";

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();

        //TODO switch-case here to set size, sprite, etc
    }

    void Update() {
        //Debug.Log(wander);
        /*
        wander -= Time.deltaTime;
        if (wander <= 0) {
            wander = 5;
            move();
        }
        */
    }

    void move() {
        rigidbody.AddForce(new Vector2(Random.Range(-2, 2) * speed, 0), ForceMode2D.Impulse);
    }
}
