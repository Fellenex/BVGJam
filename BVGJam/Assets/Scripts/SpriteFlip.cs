using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour {

    public Sprite leftFacingSprite;
    public Sprite rightFacingSprite;
    public SpriteRenderer sr;

    void Start(){
        sr = GetComponent<SpriteRenderer>();
        faceRight();
    }
    public void faceLeft() { sr.sprite = leftFacingSprite; }
    public void faceRight() { sr.sprite = rightFacingSprite; }
}
