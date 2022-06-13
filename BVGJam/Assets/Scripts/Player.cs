using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private static readonly int WalkingLeftHash = Animator.StringToHash("WalkingLeft");
    private static readonly int WalkingRightHash = Animator.StringToHash("WalkingRight");

    public float moveSpeed = 1f;
    public float jumpSpeed = 1.1f;
    public float maxSpeed = 100f;

    private Rigidbody2D rb;
    private Animator playerAnimator;

    public bool dialogOpen = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update() {
        //Don't let the player move while a dialog window is open
        if (!dialogOpen) {
            //Check for input keys to set movement
            moveLeft();
            moveRight();
            moveUp();
            
            //Check for lack of input keys to reset movement
            stopLeft();
            stopRight();
            standStill();
        }
    }

    public void moveLeft() {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            //add movement left
            playerAnimator.SetTrigger(WalkingLeftHash);
            rb.AddForce(new Vector2(-moveSpeed, 0), ForceMode2D.Force);
        }
    }

    public void moveRight() {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            //add movement right
            GetComponent<Animator>().SetTrigger(WalkingRightHash);
            rb.AddForce(new Vector2(moveSpeed, 0), ForceMode2D.Force);
        }
    }

    public void moveUp() {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
    }

    public void stopLeft() {
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow)) {
            //Stop moving left
            playerAnimator.ResetTrigger(WalkingLeftHash);
        }
    }

    public void stopRight() {
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow)) {
            playerAnimator.ResetTrigger(WalkingRightHash);
        }
    }

    public void standStill() {
        if (!Input.anyKey) {
            playerAnimator.ResetTrigger(WalkingLeftHash);
            playerAnimator.ResetTrigger(WalkingRightHash);
        }
    }

}
