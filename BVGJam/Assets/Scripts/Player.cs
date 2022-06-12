using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 2f;
    public float jumpSpeed = 1.1f;

    private Rigidbody2D rigidbody;

    public bool dialogOpen = false;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {

        //Don't let the player move while a dialog window is open
        if (!dialogOpen) {
            move();
        }
    }


    private void move() {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            //add movement left
            rigidbody.AddForce(new Vector2(-moveSpeed, 0), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            //add movement right
            rigidbody.AddForce(new Vector2(moveSpeed, 0), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) {
            //add movement up
            rigidbody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
    }

}
