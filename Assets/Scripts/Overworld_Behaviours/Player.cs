using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private readonly int WalkingLeftHash = Animator.StringToHash("WalkingLeft");
    private readonly int WalkingRightHash = Animator.StringToHash("WalkingRight");

    public float jumpForce;
    public float maxSpeed;

    private Rigidbody2D rb;
    private Animator playerAnimator;

    public bool dialogOpen = false;

    public int leftScreenBound = -2725;
    public int rightScreenBound = 13800;

    void Start() {
        jumpForce = 4f;
        maxSpeed = 30000;
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        //Make the player's stopping force a little snappier
        rb.drag = 20f;
    }

    void FixedUpdate() {
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

        //Don't let the player fall off the left/right edges of the screen!
        if (transform.position.x < leftScreenBound) {
            transform.position = new Vector3(leftScreenBound, transform.position.y, transform.position.z);
        }

        if (transform.position.x > rightScreenBound) {
            transform.position = new Vector3(rightScreenBound, transform.position.y, transform.position.z);
        }
    }

    public void moveLeft() {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            //add movement left
            playerAnimator.SetTrigger(WalkingLeftHash);

            rb.velocity = new Vector2(-1, 0) * maxSpeed * Time.fixedDeltaTime;
        }
    }

    public void moveRight() {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            //add movement right
            GetComponent<Animator>().SetTrigger(WalkingRightHash);

            rb.velocity = new Vector2(1, 0) * maxSpeed * Time.fixedDeltaTime;
        }
    }

    public void moveUp() {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
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
